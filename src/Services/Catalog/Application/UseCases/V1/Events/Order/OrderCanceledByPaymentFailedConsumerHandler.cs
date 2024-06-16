using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Order;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;

namespace Application.UseCases.V1.Events.Order;

public class OrderCanceledByPaymentFailedConsumerHandler : ICommandHandler<DomainEvent.OrderCanceledByPaymentFailed>
{
    private readonly IRepositoryBase<Domain.Entities.Ticket, Guid> _ticketRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderCanceledByPaymentFailedConsumerHandler(
        IRepositoryBase<Domain.Entities.Ticket, Guid> ticketRepository,
        IUnitOfWork unitOfWork)
    {
        _ticketRepository = ticketRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DomainEvent.OrderCanceledByPaymentFailed request, CancellationToken cancellationToken)
    {
        /*
            1. check ticketId existing
            2. update quantity of ticket
                - Update quantity of ticket
                - If Ticket being Status 'SoldOut' -> 'Available'
            3. save ticket
            4. persist into database
         */

        foreach (var orderDetailReversed in request.Details)
        {
            //1.
            var foundTicket = await _ticketRepository.FindByIdAsync(orderDetailReversed.TicketId, cancellationToken)
                ?? throw new TicketException.TicketNotFoundException(orderDetailReversed.TicketId);

            //2.
            foundTicket.ReverseQuantity(orderDetailReversed.Quantity);

            //3.
            _ticketRepository.Update(foundTicket);
        }

        //4.
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}