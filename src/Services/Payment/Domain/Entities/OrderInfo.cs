﻿using Domain.Abstractions.Entities;

namespace Domain.Entities;

public class OrderInfo : Entity<Guid>, ISoftDelete
{
    public decimal TotalPrice { get; set; }


    public bool IsDeleted { get; set; }

    public DateTimeOffset DeletedOnUtc { get; set; }
}