using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Catalog.Ticket;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;

namespace Application.UseCases.V1.Events.TicketInfo;

public class TicketDeletedConsumerHandler : ICommandHandler<DomainEvent.TicketDeleted>
{
    private readonly IRepositoryBase<Domain.Entities.TicketInfo, Guid> _ticketInfoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TicketDeletedConsumerHandler(IRepositoryBase<Domain.Entities.TicketInfo, Guid> ticketInfoRepository, IUnitOfWork unitOfWork)
    {
        _ticketInfoRepository = ticketInfoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DomainEvent.TicketDeleted request, CancellationToken cancellationToken)
    {
        // Step 01: check ticket info exists?
        var ticketHolder = await _ticketInfoRepository.FindSingleAsync(x => x.TicketId.Equals(request.Id), cancellationToken)
            ?? throw new TicketInfoException.TicketInfoNotFoundException(request.Id);

        // Step 02: remove ticket info
        _ticketInfoRepository.Remove(ticketHolder);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}