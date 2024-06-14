using Application.Abstractions;
using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Order;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;

namespace Application.UseCases.V1.Events.Order;

public class OrderCompletedConsumerHandler : ICommandHandler<DomainEvent.OrderCompleted>
{
    private readonly IRepositoryBase<Domain.Entities.Order, Guid> _orderRepository;
    private readonly IRepositoryBase<Domain.Entities.CustomerInfo, Guid> _customerRepository;
    private readonly IEmailService _emailService;

    public OrderCompletedConsumerHandler(
        IRepositoryBase<Domain.Entities.Order, Guid> orderRepository,
        IRepositoryBase<Domain.Entities.CustomerInfo, Guid> customerRepository,
        IEmailService emailService)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _emailService = emailService;
    }

    public async Task<Result> Handle(DomainEvent.OrderCompleted request, CancellationToken cancellationToken)
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

        // 3.
        await _emailService.SendEmailAsync(
            customerInfo.Email,
            "CẢM ƠN QUÝ KHÁCH ĐÃ ĐẶT VÉ Ở MUSIC CONCERT",
            $"Mã số đơn hàng của khách hàng là: {foundOrder.Id}. \n Quý khách vui lòng ghi nhận lại mã số này để tham gia sự kiện.",
            cancellationToken);

        return Result.Success();
    }
}