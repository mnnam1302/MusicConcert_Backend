using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Order;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Enumerations;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.V1.Events.Order;

public class PaymentProcessedConsumerHandler : ICommandHandler<DomainEvent.PaymentProcessed>
{
    private readonly IRepositoryBase<Domain.Entities.Order, Guid> _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PaymentProcessedConsumerHandler(
        IRepositoryBase<Domain.Entities.Order, Guid> orderRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DomainEvent.PaymentProcessed request, CancellationToken cancellationToken)
    {
        /*
            1. Get the order by order id
            2. check customerId
            3. update the order status Completed to Notification
            4. save order
         */

        //1.
        var orderHolder = await _orderRepository.FindByIdAsync(request.OrderId, cancellationToken)
            ?? throw new OrderException.OrderNotFoundException(request.OrderId);

        //2.
        if (orderHolder.CustomerInfoId != request.CustomerId)
            throw new OrderException.OrderNotBelongCustomerException(request.OrderId);

        //3.
        orderHolder.AssignPaymentProcessed(OrderStatus.OrderCompleted, request.TransactionCode);

        //4.
        _orderRepository.Update(orderHolder);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
