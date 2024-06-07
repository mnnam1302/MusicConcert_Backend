using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Order;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;
using System.Runtime.Serialization.Formatters;

namespace Application.UseCases.V1.Commands.Order;

public class CreateOrderCommandHandler : ICommandHandler<Command.CreateOrderCommand>
{
    private readonly IRepositoryBase<Domain.Entities.Order, Guid> _orderRepository;
    private readonly IRepositoryBase<Domain.Entities.CustomerInfo, Guid> _customerInfoRepository;
    private readonly IRepositoryBase<Domain.Entities.TicketInfo, Guid> _ticketInfoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderCommandHandler(
        IRepositoryBase<Domain.Entities.Order, Guid> orderRepository,
        IRepositoryBase<Domain.Entities.CustomerInfo, Guid> customerInfoRepository,
        IRepositoryBase<Domain.Entities.TicketInfo, Guid> ticketInfoRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _customerInfoRepository = customerInfoRepository;
        _ticketInfoRepository = ticketInfoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(Command.CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // Step 01: Check customer existsing
        var customerHolder = await _customerInfoRepository.FindByIdAsync(request.CustomerId, cancellationToken)
           ?? throw new CustomerInfoException.CustomerInfoNotFoundException(request.CustomerId);

        // Step 02: Check ticket existing ** Cách này không tối ưu => Gửi một lần xuống luôn
        var ticketIds = request.Details.Select(x => x.TicketId).ToList();
        
        var ticketExisting = await _ticketInfoRepository.FindAllAsync(x => ticketIds.Contains(x.Id), cancellationToken);

        if (ticketExisting.Count != ticketIds.Count)
            throw new TicketInfoException.TicketInfoNotExistsingException(
                "There is existing at least one TicketId was not found.");

        // Step 03: Create order
        var order = Domain.Entities.Order.Create(customerHolder.Id, request.Details);

        // Step 04: Persistence into database
        _orderRepository.Add(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}