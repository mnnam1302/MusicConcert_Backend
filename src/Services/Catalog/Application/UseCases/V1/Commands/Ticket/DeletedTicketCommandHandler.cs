using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Catalog.Ticket;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;

namespace Application.UseCases.V1.Commands.Ticket;

public class DeletedTicketCommandHandler : ICommandHandler<Command.DeleteTicket>
{
    private readonly IRepositoryBase<Domain.Entities.Ticket, Guid> _ticketRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeletedTicketCommandHandler(
        IRepositoryBase<Domain.Entities.Ticket, Guid> tickerRepository,
        IUnitOfWork unitOfWork)
    {
        _ticketRepository = tickerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(Command.DeleteTicket request, CancellationToken cancellationToken)
    {
        // Step 01: Check ticket existsing?
        var ticketHolder = await _ticketRepository.FindByIdAsync(request.Id, cancellationToken)
            ?? throw new TicketException.TicketNotFoundException(request.Id);

        // Step 02: Delete ticket => Update status to Discontinued & soft delete
        ticketHolder.Delete();

        // Step 03: Persistence into database
        _ticketRepository.Remove(ticketHolder);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}