using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Order;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using System.Runtime.CompilerServices;

namespace Application.UseCases.V1.Events.Order;

public class OrderCreatedConsumerHandler : ICommandHandler<DomainEvent.OrderCreated>
{
    private readonly IRepositoryBase<Domain.Entities.Ticket, Guid> _ticketRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderCreatedConsumerHandler(IRepositoryBase<Ticket, Guid> ticketRepository, IUnitOfWork unitOfWork)
    {
        _ticketRepository = ticketRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DomainEvent.OrderCreated request, CancellationToken cancellationToken)
    {
        foreach (var orderDetail in request.Details)
        {
            // Step 01: Check ticketId existsing
            var ticketHolder = await _ticketRepository.FindByIdAsync(orderDetail.TicketId, cancellationToken)
                ?? throw new TicketException.TicketNotFoundException(orderDetail.TicketId);

            // Step 02: Check quantity of ticket && Update
            ticketHolder.AssignQuantity(orderDetail.Quantity);

            _ticketRepository.Update(ticketHolder);
        }

        // Step 04: Save changes
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}