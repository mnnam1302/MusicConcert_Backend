namespace Domain.Exceptions;

public class OrderDetailsException
{
    public class OrderDetailsWithQuantityException : BadRequestException
    {
        public OrderDetailsWithQuantityException()
            : base($"The order details with Quantity must be between 1 and 5.")
        {
        }
    }
}