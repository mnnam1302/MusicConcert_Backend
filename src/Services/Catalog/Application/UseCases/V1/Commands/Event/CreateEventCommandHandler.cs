using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Catalog.Event;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;

namespace Application.UseCases.V1.Commands.Event;

public class CreateEventCommandHandler : ICommandHandler<Command.CreateEventCommand, Response.CreateEventResponse>
{
    private readonly IRepositoryBase<Domain.Entities.Event, Guid> _eventRepository;
    private readonly IRepositoryBase<Domain.Entities.OrganizationInfo, Guid> _organizationInfoRepository;
    private readonly IRepositoryBase<Domain.Entities.Category, Guid> _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateEventCommandHandler(
        IRepositoryBase<Domain.Entities.Event, Guid> eventRepository,
        IRepositoryBase<Domain.Entities.OrganizationInfo, Guid> organizationInfoRepository,
        IRepositoryBase<Domain.Entities.Category, Guid> categoryRepository,
        IUnitOfWork untiOfWork)
    {
        _eventRepository = eventRepository;
        _unitOfWork = untiOfWork;
        _organizationInfoRepository = organizationInfoRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<Response.CreateEventResponse>> Handle(Command.CreateEventCommand request, CancellationToken cancellationToken)
    {
         /*
            1. check organization existing
            2. check category existing
            3. Create event
            4. save in db
         */

        //1.
        Guid organizationId = new();
        if (request.OrganizationId.HasValue)
        {
            var organizationInfoHolder = await _organizationInfoRepository.FindByIdAsync(request.OrganizationId.Value, cancellationToken)
                ?? throw new OrganizationInfoException.OrganizationNotFoundException(request.OrganizationId.Value);

            // Using suggrorate key here
            organizationId = organizationInfoHolder.Id;
        }

        //2.
        if (request.CategoryId.HasValue)
        {
            var category = await _categoryRepository.FindByIdAsync(request.CategoryId.Value, cancellationToken)
                ?? throw new CategoryException.CategoryNotFoundException(request.CategoryId.Value);
        }

        //3.
        var @event = Domain.Entities.Event.Create(
            request.Name,
            request.Description,
            request.StartedDateOnUtc,
            request.EndedDateOnUtc,
            request.Capacity,
            request.CategoryId, 
            organizationId,
            request.MeetUrl,
            request.Adrress,
            request.District,
            request.City,
            request.Country,
            request.EventType);

        
        //4.
        _eventRepository.Add(@event);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var result = new Response.CreateEventResponse(
            @event.Id,
            @event.Name,
            @event.Description);

        return Result.Success(result);
    }
}