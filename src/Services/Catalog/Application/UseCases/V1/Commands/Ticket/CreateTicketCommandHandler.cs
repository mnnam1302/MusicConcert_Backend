using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Catalog.Ticket;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;

namespace Application.UseCases.V1.Commands.Ticket;

public class CreateTicketCommandHandler : ICommandHandler<Command.CreateTicket>
{
    private readonly IRepositoryBase<Domain.Entities.Ticket, Guid> _tickerRepository;
    private readonly IRepositoryBase<Domain.Entities.Event, Guid> _eventRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTicketCommandHandler(
        IRepositoryBase<Domain.Entities.Ticket, Guid> tickerRepository,
        IRepositoryBase<Domain.Entities.Event, Guid> eventRepository,
        IUnitOfWork unitOfWork)
    {
        _eventRepository = eventRepository;
        _tickerRepository = tickerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(Command.CreateTicket request, CancellationToken cancellationToken)
    {
        // Step 01: Check event existsing?
        var eventHolder = await _eventRepository.FindByIdAsync(request.EventId, cancellationToken)
            ?? throw new EventException.EventNotFoundException(request.EventId);

        // Step 02: Create ticket
        var ticket = Domain.Entities.Ticket.Create(request.Name, request.UnitPrice, request.UnitInStock, eventHolder.Id);

        // Step 03: Persistence into database
        _tickerRepository.Add(ticket);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}