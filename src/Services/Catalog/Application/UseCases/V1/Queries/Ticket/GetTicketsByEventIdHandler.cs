using AutoMapper;
using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Catalog.Ticket;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;

namespace Application.UseCases.V1.Queries.Ticket;

public class GetTicketsByEventIdHandler : IQueryHandler<Query.GetTicketsByEventId, Response.TicketsEventResponse>
{
    private readonly IRepositoryBase<Domain.Entities.Ticket, Guid> _ticketRepository;
    private readonly IRepositoryBase<Domain.Entities.Event, Guid> _eventRepository;
    private readonly IMapper _mapper;

    public GetTicketsByEventIdHandler(
        IRepositoryBase<Domain.Entities.Ticket, Guid> ticketRepository,
        IRepositoryBase<Domain.Entities.Event, Guid> eventRepository,
        IMapper mapper)
    {
        _ticketRepository = ticketRepository;
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<Result<Response.TicketsEventResponse>> Handle(Query.GetTicketsByEventId request, CancellationToken cancellationToken)
    {
        //1. check eventId
        var foundEvent = await _eventRepository.FindByIdAsync(request.EventId, cancellationToken)
            ?? throw new EventException.EventNotFoundException(request.EventId);

        //2. get tickets by eventId
        var tickets = await _ticketRepository.FindAllAsync(x => x.EventId == request.EventId, cancellationToken);

        //3. map to response
        var result = new Response.TicketsEventResponse
        {
            Id = foundEvent.Id,
            Name = foundEvent.Name,
            Tickets = _mapper.Map<List<Response.TicketResponse>>(tickets)
        };

        return Result.Success(result);
    }
}