using Application.Abstractions;
using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Order;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;

namespace Application.UseCases.V1.Events.Order;

public class OrderCancelledConsumerHandler : ICommandHandler<DomainEvent.OrderCancelled>
{
    private readonly IRepositoryBase<Domain.Entities.Order, Guid> _orderRepository;
    private readonly IRepositoryBase<Domain.Entities.CustomerInfo, Guid> _customerRepository;
    private readonly IEmailService _emailService;

    public OrderCancelledConsumerHandler(
        IRepositoryBase<Domain.Entities.Order, Guid> orderRepository,
        IRepositoryBase<Domain.Entities.CustomerInfo, Guid> customerRepository,
        IEmailService emailService)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _emailService = emailService;
    }

    public async Task<Result> Handle(DomainEvent.OrderCancelled request, CancellationToken cancellationToken)
    {
        /*
           1. get the order by order id => CustomerInfoId
           2. get info customer
           3. send email to customer
        */

        //1.
        var foundOrder = await _orderRepository.FindByIdAsync(request.OrderId, cancellationToken)
            ?? throw new OrderException.OrderNotFoundException(request.OrderId);

        //2.
        var customerInfo = await _customerRepository.FindByIdAsync(foundOrder.CustomerInfoId, cancellationToken)
            ?? throw new CustomerInfoException.CustomerInfoNotFoundException(foundOrder.CustomerInfoId);

        //3.
        await _emailService.SendEmailAsync(
            customerInfo.Email,
            "HỦY BỎ ĐƠN HÀNG",
            $"Rất tiếc, đơn hàng với mã số là: {foundOrder.Id} đã bị hủy bỏ vào lúc {request.TimeStamp.DateTime} với lý do `{request.Reason}`",
            cancellationToken);

        return Result.Success();
    }
}