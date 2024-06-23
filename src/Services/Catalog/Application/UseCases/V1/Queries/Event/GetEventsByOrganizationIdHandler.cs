using AutoMapper;
using Contracts.Abstractions.Message;
using Contracts.Abstractions.Paging;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Catalog.Event;
using Domain.Abstractions.Repositories;
using Domain.Enumerations;
using Domain.Exceptions;

namespace Application.UseCases.V1.Queries.Event;

public class GetEventsByOrganizationIdHandler : IQueryHandler<Query.GetEventsByOrganizationId, PagedResult<Response.EventResponse>>
{
    private readonly IRepositoryBase<Domain.Entities.Event, Guid> _eventRepository;
    private readonly IRepositoryBase<Domain.Entities.OrganizationInfo, Guid> _organizationRepository;

    private readonly IMapper _mapper;


    public GetEventsByOrganizationIdHandler(
        IRepositoryBase<Domain.Entities.Event, Guid> eventRepository,
        IRepositoryBase<Domain.Entities.OrganizationInfo, Guid> organizationRepository,
        IMapper mapper)
    {
        _eventRepository = eventRepository;
        _organizationRepository = organizationRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<Response.EventResponse>>> Handle(Query.GetEventsByOrganizationId request, CancellationToken cancellationToken)
    {
        /*
            1. check oranizationId existsing
            2. paging
            3. mapping
         */

        // 1.
        var events = string.IsNullOrEmpty(request.City)
            ? _eventRepository.FindAll(x => x.OrganizationInfoId == request.OrganizationId && x.Status == EventStatus.Published)
            : _eventRepository.FindAll(x => 
                        x.OrganizationInfoId.Equals(request.OrganizationId)
                        && x.Id != request.eventId
                        && x.City.Equals(request.City)
                        && x.Status == EventStatus.Published);

        // 2.
        var pagedEvents = await PagedResult<Domain.Entities.Event>.CreateAsync(events, request.PageIndex, request.PageSize, cancellationToken);

        // 3.
        var result = _mapper.Map<PagedResult<Response.EventResponse>>(pagedEvents);

        return result;
    }
}