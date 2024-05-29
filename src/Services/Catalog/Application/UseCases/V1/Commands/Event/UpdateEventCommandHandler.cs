using Application.Abstractions;
using Application.Exceptions;
using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Catalog.Event;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;

namespace Application.UseCases.V1.Commands.Event;

public class UpdateEventCommandHandler : ICommandHandler<Command.UpdateEventCommand>
{
    private readonly IRepositoryBase<Domain.Entities.Event, Guid> _eventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFirebaseService _firebaseService;

    public UpdateEventCommandHandler(
        IRepositoryBase<Domain.Entities.Event, Guid> eventRepository, 
        IUnitOfWork unitOfWork,
        IFirebaseService firebaseService)
    {
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
        _firebaseService = firebaseService;
    }


    public async Task<Result> Handle(Command.UpdateEventCommand request, CancellationToken cancellationToken)
    {
        // Step 01: Check event existing?
        var holderEvent = await _eventRepository.FindByIdAsync(request.Id, cancellationToken)
            ?? throw new EventException.EventNotFoundException(request.Id);

        // Step 02: Handle images
        if (request.LogoImage is not null || request.LayoutImage is not null)
        {
            // 2.01: Authenticate
            var isAuthenticated = await _firebaseService.Authentication();

            if (!isAuthenticated)
                throw new FirebaseException.FireBaseAuthenticateException();

            // 2.02: Upload images
            if (request.LogoImage is not null)
            {
                var logoImageUrl = await _firebaseService.UploadImage(request.LogoImage);
                holderEvent.UpdateLogoImage(logoImageUrl); // Domain driven design
            }

            if (request.LayoutImage is not null)
            {
                var layoutImageUrl = await _firebaseService.UploadImage(request.LayoutImage);
                holderEvent.UpdateLayoutImage(layoutImageUrl); // Domain driven design
            }
        }

        // Step 03: Update event
        holderEvent.Update(request.Name, request.Description, request.IsPublished);
        
        // Step 04: Persistence into DB
        _eventRepository.Update(holderEvent);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}