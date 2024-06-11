using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Payment;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;

namespace Application.UseCases.V1.Commands.Invoice;

public class PaymentInvoiceCommandHandler : ICommandHandler<Command.PaymentInvoiceCommand>
{
    private readonly IRepositoryBase<Domain.Entities.Invoice, Guid> _invoiceRepository;
    private readonly IRepositoryBase<Domain.Entities.OrderInfo, Guid> _orderRepository;
    private readonly IRepositoryBase<Domain.Entities.CustomerInfo, Guid> _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PaymentInvoiceCommandHandler(
        IRepositoryBase<Domain.Entities.Invoice, Guid> invoiceRepository, 
        IUnitOfWork unitOfWork,
        IRepositoryBase<Domain.Entities.OrderInfo, Guid> orderRepository,
        IRepositoryBase<Domain.Entities.CustomerInfo, Guid> customerRepository)
    {
        _invoiceRepository = invoiceRepository;
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
    }


    public async Task<Result> Handle(Command.PaymentInvoiceCommand request, CancellationToken cancellationToken)
    {
        /*
            1. check invoiceId exists
            2. check order exists
            3. check customer exists
            4. update invoice with transactionCode and raise event
            5. commit transaction
         */

        //1.
        var invoice = await _invoiceRepository.FindByIdAsync(request.InvoiceId, cancellationToken)
            ?? throw new InvoiceException.InvoiceNotFoundException(request.InvoiceId);

        //2.
        var order = await _orderRepository.FindByIdAsync(request.OrderId, cancellationToken)
            ?? throw new OrderInfoException.OrderInfoNotFoundException(request.OrderId);

        //3.
        var customer = await _customerRepository.FindByIdAsync(request.CustomerId, cancellationToken)
            ?? throw new CustomerInfoException.CustomerInfoNotFoundException(request.CustomerId);

        //4.
        invoice.PaymentProcessedInvoice(request.TransactionCode);

        //5.
        _invoiceRepository.Update(invoice);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}