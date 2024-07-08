using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Catalog.Ticket;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;

namespace Application.UseCases.V1.Events.TicketInfo;

public class TicketCreatedConsumerHandler : ICommandHandler<DomainEvent.TicketCreated>
{
    private readonly IRepositoryBase<Domain.Entities.TicketInfo, Guid> _ticketInfoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TicketCreatedConsumerHandler(IRepositoryBase<Domain.Entities.TicketInfo, Guid> ticketInfoRepository, IUnitOfWork unitOfWork)
    {
        _ticketInfoRepository = ticketInfoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DomainEvent.TicketCreated request, CancellationToken cancellationToken)
    {
        // Step 01: check ticket info exists?
        var ticketHolder = await _ticketInfoRepository.FindByIdAsync(request.Id, cancellationToken);
        //var ticketHolder = await _ticketInfoRepository.FindSingleAsync(x => x.TicketId.Equals(request.Id), cancellationToken);

        if (ticketHolder is not null)
            throw new TicketInfoException.TicketInfoAlreadyExistsException(request.Id);

        // Step 02: create ticket info
        var ticketInfo = new Domain.Entities.TicketInfo(request.Id, request.Name);

        // Step 03: persist into database
        _ticketInfoRepository.Add(ticketInfo);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}