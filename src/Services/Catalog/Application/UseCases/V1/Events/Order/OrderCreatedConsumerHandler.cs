using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Order;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MassTransit;

namespace Application.UseCases.V1.Events.Order;

public class OrderCreatedConsumerHandler : ICommandHandler<DomainEvent.OrderCreated>
{
    private readonly IRepositoryBase<Domain.Entities.Ticket, Guid> _ticketRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IUnitOfWork _unitOfWork;

    public OrderCreatedConsumerHandler(
        IRepositoryBase<Ticket, Guid> ticketRepository, 
        IUnitOfWork unitOfWork, 
        IPublishEndpoint publishEndpoint)
    {
        _ticketRepository = ticketRepository;
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Result> Handle(DomainEvent.OrderCreated request, CancellationToken cancellationToken)
    {
        /*
            1. check each TicketId existsing            -> If failed -> StockReversedFailed
            2. check Ticket's quantity is enough        -> If failed -> StockReversedFailed
            3. update quantity of ticket
            4. save into database
            5. publish event OrderValidated - Success

            Note: Should't not throw error here
         */
        foreach (var orderDetail in request.Details)
        {
            // 1.
            var ticketHolder = await _ticketRepository.FindByIdAsync(orderDetail.TicketId, cancellationToken);
            
            if (ticketHolder is null)
            {
                var stockkReversedFailed = new Contracts.Services.V1.Order.DomainEvent.StockReversedFailed
                {
                    EventId = Guid.NewGuid(),
                    TimeStamp = DateTimeOffset.UtcNow,
                    OrderId = request.OrderId,
                    CustomerId = request.CustomerId,
                    Reason = $"Ticket with Id: {orderDetail.TicketId} is not existing.",
                };

                await _publishEndpoint.Publish(stockkReversedFailed, context => context.CorrelationId = context.Message.OrderId);
                return Result.Success();
            }

            // 2.
            if (ticketHolder.UnitInStock < orderDetail.Quantity)
            {
                var stockkReversedFailed = new Contracts.Services.V1.Order.DomainEvent.StockReversedFailed
                {
                    EventId = Guid.NewGuid(),
                    TimeStamp = DateTimeOffset.UtcNow,
                    OrderId = request.OrderId,
                    CustomerId = request.CustomerId,
                    Reason = $"Ticket `{ticketHolder.Name}` với Id: {ticketHolder.Id} is not enough.",
                };

                await _publishEndpoint.Publish(stockkReversedFailed, context => context.CorrelationId = context.Message.OrderId);
                return Result.Success();
            }

            // 3.
            ticketHolder.AssignQuantity(orderDetail.Quantity);
            _ticketRepository.Update(ticketHolder);
        }

        // 4.
        var stockReversed = new DomainEvent.StockReversed
        {
            EventId = Guid.NewGuid(),
            TimeStamp = DateTimeOffset.UtcNow,
            OrderId = request.OrderId
        };

        await _publishEndpoint.Publish(stockReversed, context => context.CorrelationId = context.Message.OrderId);

        // 5.
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}