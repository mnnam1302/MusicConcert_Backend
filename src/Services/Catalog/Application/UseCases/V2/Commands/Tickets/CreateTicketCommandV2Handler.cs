using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Catalog.Ticket;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;

namespace Application.UseCases.V2.Commands.Tickets;

public class CreateTicketCommandV2Handler : ICommandHandler<Command.CreateTicketCommandV2>
{
    private readonly IRepositoryBase<Domain.Entities.Ticket, Guid> _tickerRepository;
    private readonly IRepositoryBase<Domain.Entities.Event, Guid> _eventRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTicketCommandV2Handler(
        IRepositoryBase<Domain.Entities.Ticket, Guid> tickerRepository,
        IRepositoryBase<Domain.Entities.Event, Guid> eventRepository,
        IUnitOfWork unitOfWork)
    {
        _eventRepository = eventRepository;
        _tickerRepository = tickerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(Command.CreateTicketCommandV2 request, CancellationToken cancellationToken)
    {
        /*
            1. Check event existsing?
            2. Create ticket
            3. Persistence into database
        */

        // 1.
        var foundEvent = await _eventRepository.FindByIdAsync(request.EventId, cancellationToken)
            ?? throw new EventException.EventNotFoundException(request.EventId);

        // 2.
        foreach (var ticketItem in request.Tickets)
        {
            var ticket = Domain.Entities.Ticket.Create(
                ticketItem.Name, 
                ticketItem.UnitPrice, 
                ticketItem.UnitInStock, 
                foundEvent.Id);

            _tickerRepository.Add(ticket);
        }

        // 3.
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}