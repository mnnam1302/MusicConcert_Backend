﻿namespace Domain.Exceptions;

public static class OrderException
{
    public class OrderFieldException : BadRequestException
    {
        public OrderFieldException(string orderField)
            : base($"The order with Field {orderField} is not correct.")
        {
        }
    }

    public class OrderNotFoundException : NotFoundException
    {
        public OrderNotFoundException(Guid orderId)
            : base($"The order with Id {orderId} was not found.")
        {
        }
    }

    public class OrderNotBelongCustomerException : BadRequestException
    {
        public OrderNotBelongCustomerException(Guid orderId)
            : base($"The order with Id {orderId} is not belong to the customer.")
        {
        }
    }
}