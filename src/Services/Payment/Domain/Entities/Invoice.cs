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

        //if (orderId.HasValue)
        //    invoice.AssignOrderInfo(orderId.Value);

        //if (customerId.HasValue)
        //    invoice.AssignCustomerInfo(customerId.Value);
    
        return invoice;
    }

    private Invoice AssignOrderInfo(Guid orderId)
    {
        OrderInfoId = orderId;
        return this;
    }

    private Invoice AssignCustomerInfo(Guid customerId)
    {
        CustomerInfoId = customerId;
        return this;
    }


    public decimal SubTotal { get; private set; }
    public decimal Discount { get; private set; }
    public decimal Tax { get; private set; }
    public decimal TotalPrice { get; private set; }
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