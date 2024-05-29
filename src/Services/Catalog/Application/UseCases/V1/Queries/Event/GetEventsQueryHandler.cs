using AutoMapper;
using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Catalog.Event;
using Domain.Abstractions.Repositories;

namespace Application.UseCases.V1.Queries.Event;

public class GetEventsQueryHandler : IQueryHandler<Query.GetEventsQuery, List<Response.EventResponse>>
{
    private readonly IRepositoryBase<Domain.Entities.Event, Guid> _eventRepository;
    private readonly IMapper _mapper;


    public GetEventsQueryHandler(
        IRepositoryBase<Domain.Entities.Event, Guid> eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<Response.EventResponse>>> Handle(Query.GetEventsQuery request, CancellationToken cancellationToken)
    {
        var holderEvents = await _eventRepository.FindAllAsync(cancellationToken: cancellationToken);

        var result = _mapper.Map<List<Response.EventResponse>>(holderEvents);

        return Result.Success(result);
    }
}