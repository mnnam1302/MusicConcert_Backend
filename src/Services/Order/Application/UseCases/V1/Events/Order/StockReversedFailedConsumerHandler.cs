﻿using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Order;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Enumerations;
using Domain.Exceptions;

namespace Application.UseCases.V1.Events.Order;

public class StockReversedFailedConsumerHandler : ICommandHandler<DomainEvent.StockReversedFailed>
{
    private readonly IRepositoryBase<Domain.Entities.Order, Guid> _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public StockReversedFailedConsumerHandler(
        IRepositoryBase<Domain.Entities.Order, Guid> orderRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DomainEvent.StockReversedFailed request, CancellationToken cancellationToken)
    {
        // Step 01: Check order existsing?
        var orderHolder = await _orderRepository.FindByIdAsync(request.OrderId, cancellationToken)
            ?? throw new OrderException.OrderNotFoundException(request.OrderId);

        // Step 02: Update status 'OrderCancelled' -> RaiseEvent: OrderCancelled (Notification Service)
        orderHolder.AssignValidatedFailed(OrderStatus.OrderCancelled, request.Reason);

        // Step 03: Persistence into database
        _orderRepository.Update(orderHolder);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}