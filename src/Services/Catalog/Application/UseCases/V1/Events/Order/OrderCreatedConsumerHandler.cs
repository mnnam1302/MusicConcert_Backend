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

    /// <summary>
    /// Check ticketId existsing
    /// Check quantity of ticket
    ///     Update quantity of ticket
    ///     If extsing at least one Ticket is not enough => Publish event OrderValidatedFailed
    ///         Lưu ý: 
    ///             + Ví dụ User đặt Ticket 3 vé, nhưng chỉ còn 2 vé => không đủ
    ///             + Chỉ publish event Validated-Failed và 2 vé này vẫn bth, người sau nếu thỏa thì đặt vé
    ///             + Hạn chế throw exception nếu thật sự không cần thiết
    /// Publish event OrderValidated
    /// Persistence into database
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="TicketException.TicketNotFoundException"></exception>
    public async Task<Result> Handle(DomainEvent.OrderCreated request, CancellationToken cancellationToken)
    {
        foreach (var orderDetail in request.Details)
        {
            // Step 01: Check ticketId existsing
            var ticketHolder = await _ticketRepository.FindByIdAsync(orderDetail.TicketId, cancellationToken);
            
            if (ticketHolder is null)
            {
                var stockkReversedFailed = new Contracts.Services.V1.Order.DomainEvent.StockReversedFailed
                {
                    EventId = Guid.NewGuid(),
                    TimeStamp = DateTimeOffset.UtcNow,
                    OrderId = request.OrderId,
                    CustomerId = request.CustomerId,
                    Reason = $"TicketId: {orderDetail.TicketId} is not existing.",
                };

                await _publishEndpoint.Publish(stockkReversedFailed, cancellationToken);
                return Result.Success();
            }

            // Step 02: Check quantity
            if (ticketHolder.UnitInStock < orderDetail.Quantity)
            {
                var stockkReversedFailed = new Contracts.Services.V1.Order.DomainEvent.StockReversedFailed
                {
                    EventId = Guid.NewGuid(),
                    TimeStamp = DateTimeOffset.UtcNow,
                    OrderId = request.OrderId,
                    CustomerId = request.CustomerId,
                    Reason = $"TicketId: {ticketHolder.Id} and Name: {ticketHolder.Name} with Quantity {ticketHolder.UnitInStock} is not enough.",
                };

                await _publishEndpoint.Publish(stockkReversedFailed, cancellationToken);
                return Result.Success();
            }

            // Step 03: Update quantity cho OrderDetail này => xử lý OrderDetail tiếp theo
            ticketHolder.AssignQuantity(orderDetail.Quantity);
            _ticketRepository.Update(ticketHolder);
        }

        // Step 04: Publish event OrderValidated - Success => Order and all OrderDetails passed
        var stockReversed = new DomainEvent.StockReversed
        {
            EventId = Guid.NewGuid(),
            TimeStamp = DateTimeOffset.UtcNow,
            OrderId = request.OrderId
        };

        await _publishEndpoint.Publish(stockReversed, cancellationToken);

        // Step 04: Save changes
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}