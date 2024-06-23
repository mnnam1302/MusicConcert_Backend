using AutoMapper;
using Contracts.Abstractions.Message;
using Contracts.Abstractions.Paging;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Catalog.Event;
using Domain.Abstractions.Repositories;
using Domain.Enumerations;
using Domain.Exceptions;

namespace Application.UseCases.V1.Queries.Event;

public class GetEventsBasedOnStatusByOrganizationIdHandler
    : IQueryHandler<Query.GetEventsBasedOnStatusByOrganizationId, PagedResult<Response.EventResponse>>
{
    private readonly IRepositoryBase<Domain.Entities.Event, Guid> _eventRepository;
    private readonly IRepositoryBase<Domain.Entities.OrganizationInfo, Guid> _organizationRepository;
    private readonly IMapper _mapper;

    public GetEventsBasedOnStatusByOrganizationIdHandler(
        IRepositoryBase<Domain.Entities.Event, Guid> eventRepository,
        IRepositoryBase<Domain.Entities.OrganizationInfo, Guid> organizationRepository,
        IMapper mapper)
    {
        _eventRepository = eventRepository;
        _organizationRepository = organizationRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<Response.EventResponse>>> Handle(Query.GetEventsBasedOnStatusByOrganizationId request, CancellationToken cancellationToken)
    {
        /*
            1. check oranizationId existsing
            2. check status
            3. paging
            4. mapping
        */

        // 1.
        var foundOrganizationInfo = await _organizationRepository.FindByIdAsync(request.OrganizationId, cancellationToken)
            ?? throw new OrganizationInfoException.OrganizationNotFoundException(request.OrganizationId);
        
        // Get all events include Published, Unpublished, Cancelled
        var events = _eventRepository.FindAll(x => x.OrganizationInfoId == request.OrganizationId);

        // 2.
        if (!string.IsNullOrEmpty(request.Status))
        {
            var status = ConvertStringToEventStatus(request.Status);

            events = events.Where(x => x.Status == status);
        }

        // 3.
        var pagedEvents = await PagedResult<Domain.Entities.Event>.CreateAsync(events, request.PageIndex, request.PageSize, cancellationToken);

        // 4.
        var result = _mapper.Map<PagedResult<Response.EventResponse>>(pagedEvents);
        return Result.Success(result);
    }

    private string ConvertStringToEventStatus(string eventStatus)
        => eventStatus.ToLower() switch
        {
            "published" => EventStatus.Published,
            "nonpublished" => EventStatus.NonPublished,
            "cancelled" => EventStatus.Cancelled,
            _ => EventStatus.Published
        };
}