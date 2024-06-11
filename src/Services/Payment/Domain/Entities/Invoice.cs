using Contracts.Services.V1.Order;
using Domain.Abstractions.Aggregates;
using Domain.Abstractions.Entities;
using Domain.Enumerations;

namespace Domain.Entities;

public class Invoice : AggregateRoot<Guid>, IAuditable, ISoftDeleted
{
    protected Invoice()
    {
    }

    private Invoice(Guid id, Guid orderId, Guid customerId, decimal subTotal, decimal discount, decimal tax, decimal totalPrice)
    {
        Id = id;
        OrderInfoId = orderId;
        CustomerInfoId = customerId;
        SubTotal = subTotal;
        Discount = discount;
        Tax = tax;
        TotalPrice = totalPrice;
    }

    public static Invoice Create(Guid orderId, Guid customerId, decimal subTotal, decimal discount = 0, decimal tax = 0)
    {
        var totalPrice = subTotal - discount + tax;

        var invoice = new Invoice(Guid.NewGuid(), orderId, customerId, subTotal, discount, tax, totalPrice);
    
        return invoice;
    }

    public Invoice PaymentProcessedInvoice(string transactionCode)
    {
        TransactionCode = transactionCode;
        PaymentedOnUtc = DateTimeOffset.UtcNow;

        Status = InvoiceStatus.Paid;

        // RaiseEvent PaymentProccessed
        RaiseDomainEvent(new DomainEvent.PaymentProcessed
        {
            EventId = Guid.NewGuid(),
            TimeStamp = DateTimeOffset.UtcNow,
            OrderId = OrderInfoId.Value,
            CustomerId = CustomerInfoId.Value,
            TransactionCode = this.TransactionCode
        });

        return this;
    }

    public Invoice PaymentProcessFailedInvoice()
    {
        Status = InvoiceStatus.Cancelled;

        // RaiseEvent PaymentProccessedFailed
        RaiseDomainEvent(new DomainEvent.PaymentProcessedFailed
        {
            EventId = Guid.NewGuid(),
            TimeStamp = DateTimeOffset.UtcNow,
            OrderId = OrderInfoId.Value,
            CustomerId = CustomerInfoId.Value,
            Reason = "Customer cancel payment.",
            TotalMoney = TotalPrice
        });

        return this;
    }

    public Invoice AssignOrderInfo(Guid orderId)
    {
        OrderInfoId = orderId;
        return this;
    }

    public Invoice AssignCustomerInfo(Guid customerId)
    {
        CustomerInfoId = customerId;
        return this;
    }


    public decimal SubTotal { get; private set; }
    public decimal Discount { get; private set; }
    public decimal Tax { get; private set; }
    public decimal TotalPrice { get; private set; }

    public string? TransactionCode { get; private set; }
    public DateTimeOffset? PaymentedOnUtc { get; private set; }
    public string Status { get; private set; } = InvoiceStatus.Pending;
    public Guid? CustomerInfoId { get; set; }
    public virtual CustomerInfo? CustomerInfo { get; set; }
    public Guid? OrderInfoId { get; set; }
    public virtual OrderInfo? OrderInfo { get; set; }

    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset DeletedOnUtc { get; set; }
}