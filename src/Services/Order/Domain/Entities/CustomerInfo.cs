﻿using Domain.Abstractions.Entities;

namespace Domain.Entities;

public class CustomerInfo : Entity<Guid>, IAuditable, ISoftDeleted
{
    public CustomerInfo()
    {
    }

    public CustomerInfo(Guid customerId)
    {
        Id = customerId; // Surrogate key in Service Order
        //CustomerId = customerId;
    }

    //public Guid CustomerId { get; private set; }

    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset DeletedOnUtc { get; set; }
}