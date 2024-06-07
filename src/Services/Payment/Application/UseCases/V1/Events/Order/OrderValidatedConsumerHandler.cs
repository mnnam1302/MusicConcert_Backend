using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Order;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.UseCases.V1.Events.Order;

public class OrderValidatedConsumerHandler : ICommandHandler<DomainEvent.OrderValidated>
{
    private readonly IRepositoryBase<Domain.Entities.Invoice, Guid> _invoiceRepository;
    private readonly IRepositoryBase<Domain.Entities.OrderInfo, Guid> _orderInfoRepository;
    private readonly IRepositoryBase<Domain.Entities.CustomerInfo, Guid> _customerInfoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderValidatedConsumerHandler(
        IRepositoryBase<Invoice, Guid> invoiceRepository,
        IRepositoryBase<Domain.Entities.OrderInfo, Guid> orderRepository,
        IRepositoryBase<Domain.Entities.CustomerInfo, Guid> customerRepository,
        IUnitOfWork unitOfWork)
    {
        _invoiceRepository = invoiceRepository;
        _orderInfoRepository = orderRepository;
        _customerInfoRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DomainEvent.OrderValidated request, CancellationToken cancellationToken)
    {
        // Step 01: Check OrderInfo existing -> If exists, throw exception
        var orderHolder = await _orderInfoRepository.FindByIdAsync(request.OrderId, cancellationToken);

        if (orderHolder is not null)
            throw new OrderInfoException.OrderInfoAlreadyExistsException(request.OrderId);

        // Step 02: Check Customer existing
        var customerHolder = await _customerInfoRepository.FindByIdAsync(request.CustomerId, cancellationToken)
            ?? throw new CustomerInfoException.CustomerInfoNotFoundException(request.CustomerId);

        // Step 03: Create OrderInfo
        var orderInfo = new OrderInfo(request.OrderId);

        // Step 03: Create Invoice
        var invoice = Invoice.Create(orderInfo.Id, customerHolder.Id, request.TotalAmount);

        // Step 04: Persistence into database
        _orderInfoRepository.Add(orderInfo);
        _invoiceRepository.Add(invoice);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}