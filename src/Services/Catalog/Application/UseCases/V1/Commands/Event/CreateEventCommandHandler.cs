﻿using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Catalog.Event;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;

namespace Application.UseCases.V1.Commands.Event;

public class CreateEventCommandHandler : ICommandHandler<Command.CreateEventCommand>
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

    public async Task<Result> Handle(Command.CreateEventCommand request, CancellationToken cancellationToken)
    {
        // Step 01: check organization existing?
        Guid organizationId = new();
        if (request.OrganizationId.HasValue)
        {
            var organizationInfoHolder = await _organizationInfoRepository.FindSingleAsync(
                x => x.OrganizaitonId.Equals(request.OrganizationId.Value), cancellationToken)
                ?? throw new OrganizationInfoException.OrganizationNotFoundException(request.OrganizationId.Value);

            // Using suggrorate key here
            organizationId = organizationInfoHolder.Id;
        }

        // Step 02: check category existing?
        if (request.CategoryId.HasValue)
        {
            var category = await _categoryRepository.FindByIdAsync(request.CategoryId.Value, cancellationToken)
                ?? throw new CategoryException.CategoryNotFoundException(request.CategoryId.Value);
        }

        // Step 03: Create event
        var @event = Domain.Entities.Event.Create(
            request.Name,
            request.Description,
            request.StartedDateOnUtc,
            request.EndedDateOnUtc,
            request.Capacity,
            request.CategoryId, 
            //request.OrganizationId,
            organizationId,
            request.MeetUrl,
            request.Adrress,
            request.District,
            request.City,
            request.Country,
            request.EventType);

        
        // Peristence into DB
        _eventRepository.Add(@event);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}