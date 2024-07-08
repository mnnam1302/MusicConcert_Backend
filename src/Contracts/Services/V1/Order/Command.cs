﻿using Contracts.Abstractions.Message;

namespace Contracts.Services.V1.Order;

public static class Command
{
    // ============= OrderDetail =============
    public record CreateOrderCommand : ICommand<Response.CreateOrderResponse>
    {
        public Guid CustomerId { get; init; }
        public List<OrderDetail> Details { get; init; } = new();
    }


    // ============= OrderDetail =============
    public readonly record struct OrderDetail(Guid TicketId, decimal UnitPrice, int Quantity);
}