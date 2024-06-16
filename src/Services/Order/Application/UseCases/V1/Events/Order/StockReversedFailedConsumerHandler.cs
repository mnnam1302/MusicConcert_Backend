using Contracts.Abstractions.Message;
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
        /*
            1. check order existing
            2. update Status OrderCancelled
            3. save order
         */

        //1.
        var orderHolder = await _orderRepository.FindByIdAsync(request.OrderId, cancellationToken)
            ?? throw new OrderException.OrderNotFoundException(request.OrderId);

        //2.
        // V1 Có nên RaiseEvent Cancelled here => Theo mình nên retrun Result attaced Reason for User
        orderHolder.AssignValidatedFailed(OrderStatus.OrderCancelled, request.Reason);

        // V2: ý tưởng ổn -> nhưng Response nó đã trả về lúc Order => Chỗ này consumer nên không trả về
        // Solution gửi vô một cái Hub dùng socketio thông báo cho người dùng ở ứng dụng
        //orderHolder.AssignValidatedFailed(OrderStatus.OrderCancelled);

        //3.
        _orderRepository.Update(orderHolder);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}