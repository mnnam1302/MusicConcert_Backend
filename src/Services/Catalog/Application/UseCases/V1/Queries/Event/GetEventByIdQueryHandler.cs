using AutoMapper;
using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Catalog.Event;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;

namespace Application.UseCases.V1.Queries.Event;

public class GetEventByIdQueryHandler : IQueryHandler<Query.GetEventByIdQuery, Response.EventDetailsReponse>
{
    private readonly IRepositoryBase<Domain.Entities.Event, Guid> _eventRepository;
    private readonly IMapper _mapper;

    public GetEventByIdQueryHandler(
        IRepositoryBase<Domain.Entities.Event, Guid> eventRepository, 
        IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<Result<Response.EventDetailsReponse>> Handle(Query.GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        //1. Check event existsing
        var holderEvent = await _eventRepository.FindByIdAsync(
                request.Id,
                cancellationToken,
                e => e.Category,
                e => e.OrganizationInfo,
                e => e.Tickets)
            ?? throw new EventException.EventNotFoundException(request.Id);

        //2. mapping
        var result = _mapper.Map<Response.EventDetailsReponse>(holderEvent);

        return Result.Success(result);
    }
}