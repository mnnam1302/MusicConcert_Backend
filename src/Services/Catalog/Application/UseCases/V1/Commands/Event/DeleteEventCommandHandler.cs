using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Catalog.Event;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;

namespace Application.UseCases.V1.Commands.Event;

public class DeleteEventCommandHandler : ICommandHandler<Command.DeleteEventCommand>
{
    private readonly IRepositoryBase<Domain.Entities.Event, Guid> _eventRepository;
    private readonly IUnitOfWork _unitOfWork;
    public DeleteEventCommandHandler(IRepositoryBase<Domain.Entities.Event, Guid> eventRepository, IUnitOfWork unitOfWork)
    {
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(Command.DeleteEventCommand request, CancellationToken cancellationToken)
    {
        // Step 01: Check event existsing?
        var holderEvent = await _eventRepository.FindByIdAsync(request.Id, cancellationToken)
            ?? throw new EventException.EventNotFoundException(request.Id);

        // Step 02: Handle business - DDD
        holderEvent.Delete();

        // Step 03: Persistence into database
        _eventRepository.Remove(holderEvent);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}