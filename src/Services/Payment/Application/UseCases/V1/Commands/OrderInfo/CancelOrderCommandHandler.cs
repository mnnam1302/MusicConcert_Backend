using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Payment.OrderInfo;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Enumerations;
using Domain.Exceptions;

namespace Application.UseCases.V1.Commands.OrderInfo;

public class CancelOrderCommandHandler : ICommandHandler<Command.CancelOrderCommand>
{
    private readonly IRepositoryBase<Domain.Entities.Invoice, Guid> _invoiceRepository;
    private readonly IRepositoryBase<Domain.Entities.OrderInfo, Guid> _orderRepository;
    private readonly IRepositoryBase<Domain.Entities.CustomerInfo, Guid> _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelOrderCommandHandler(
        IRepositoryBase<Domain.Entities.Invoice, Guid> invoiceRepository,
        IRepositoryBase<Domain.Entities.OrderInfo, Guid> orderRepository,
        IRepositoryBase<Domain.Entities.CustomerInfo, Guid> customerRepository,
        IUnitOfWork unitOfWork)
    {
        _invoiceRepository = invoiceRepository;
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(Command.CancelOrderCommand request, CancellationToken cancellationToken)
    {
        /*
            1. check order existing
            2. get invoice for this Order -> ensure status is 'Pending'
            3. check customer existing
            4. update invoice with transactionCode, status && raise event
            5. save into database
         */

        // 1.
        var foundOrder = await _orderRepository.FindByIdAsync(request.OrderId.Value, cancellationToken)
            ?? throw new OrderInfoException.OrderInfoNotFoundException(request.OrderId.Value);
        
        // 2.
        var foundInvoice = await _invoiceRepository.FindSingleAsync(x => x.OrderInfoId == request.OrderId.Value, cancellationToken)
            ?? throw new InvoiceException.InvoiceNotFoundException();

        if (foundInvoice.Status == InvoiceStatus.Cancelled || foundInvoice.Status == InvoiceStatus.Paid)
            throw new InvoiceException.InvoiceFieldException(nameof(foundInvoice.Status));

        // 3.
        if (foundInvoice.CustomerInfoId != request.CustomerId)
            throw new InvoiceException.InvoiceFieldException(nameof(foundInvoice.CustomerInfoId));

        // 4.
        foundInvoice.PaymentProcessFailedInvoice();

        // 5.
        _invoiceRepository.Update(foundInvoice);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}