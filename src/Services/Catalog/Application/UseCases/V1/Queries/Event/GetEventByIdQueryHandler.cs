using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Catalog.Event;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;

namespace Application.UseCases.V1.Queries.Event;

public class GetEventByIdQueryHandler : IQueryHandler<Query.GetEventByIdQuery, Response.EventDetailsReponse>
{
    private readonly IRepositoryBase<Domain.Entities.Event, Guid> _eventRepository;
    private readonly IRepositoryBase<Domain.Entities.OrganizationInfo, Guid> _organizationRepository;


    public GetEventByIdQueryHandler(
        IRepositoryBase<Domain.Entities.Event, Guid> eventRepository, 
        IRepositoryBase<Domain.Entities.OrganizationInfo, Guid> organizationRepository)
    {
        _eventRepository = eventRepository;
        _organizationRepository = organizationRepository;
    }

    public async Task<Result<Response.EventDetailsReponse>> Handle(Query.GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        // Step 01: Check event existsing
        var holderEvent = await _eventRepository.FindByIdAsync(request.Id, cancellationToken, e => e.Category)
            ?? throw new EventException.EventNotFoundException(request.Id);

        // Step 02: Get organization info
        var holderOrganizationInfo = await _organizationRepository.FindByIdAsync(holderEvent.OrganizationInfoId, cancellationToken)
            ?? throw new OrganizationInfoException.OrganizationNotFoundException(holderEvent.OrganizationInfoId);

        // Step 03: Make response
        var result = new Response.EventDetailsReponse(
            holderEvent.Id,
            holderEvent.Name,
            holderEvent.Description,
            holderEvent.Category?.Name ?? string.Empty,
            holderEvent.EventType,
            holderOrganizationInfo.Name,
            holderEvent.LogoImage,
            holderEvent.LayoutImage,                                                                                           
            holderEvent.StartedOnUtc,
            holderEvent.EndedOnUtc,
            holderEvent.Capacity,
            holderEvent.MeetUrl,
            holderEvent.Address,
            holderEvent.District,
            holderEvent.City,
            holderEvent.Country);

        return Result.Success(result);
    }
}