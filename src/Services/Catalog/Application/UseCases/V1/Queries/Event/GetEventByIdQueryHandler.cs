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
        var holderEvent = await _eventRepository.FindByIdAsync(
                request.Id,
                cancellationToken,
                e => e.Category,
                e => e.OrganizationInfo)
            ?? throw new EventException.EventNotFoundException(request.Id);

        // Step 03: Make response
        var result = new Response.EventDetailsReponse
        {
            Id = holderEvent.Id,
            Name = holderEvent.Name,
            Description = holderEvent.Description,
            CategoryName = holderEvent.Category?.Name ?? string.Empty,
            EventType = holderEvent.EventType,
            OrganizationName = holderEvent.OrganizationInfo is not null ? holderEvent.OrganizationInfo.Name : string.Empty,
            LogoImage = holderEvent.LogoImage,
            LayoutImage = holderEvent.LayoutImage,
            StartedDateOnUtc = holderEvent.StartedOnUtc,
            EndedDateOnUtc = holderEvent.EndedOnUtc,
            Capacity = holderEvent.Capacity,
            MeetUrl = holderEvent.MeetUrl,
            Addrees = holderEvent.Address,
            District = holderEvent.District,
            City = holderEvent.City,
            Country = holderEvent.Country
        };

        return Result.Success(result);
    }
}