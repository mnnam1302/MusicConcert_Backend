namespace Domain.Exceptions;

public static class OrderException
{
    public class OrderFieldException : BadRequestException
    {
        public OrderFieldException(string orderField)
            : base($"The order with Field {orderField} is not correct.")
        {
        }
    }
}