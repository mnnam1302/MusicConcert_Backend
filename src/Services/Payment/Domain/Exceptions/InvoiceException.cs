namespace Domain.Exceptions;

public static class InvoiceException
{
    public class InvoiceNotFoundException : NotFoundException
    {
        public InvoiceNotFoundException()
            : base("Invoice was not found.")
        {
        }

        public InvoiceNotFoundException(Guid invoiceId)
            : base($"Invoice with Id {invoiceId} was not found.")
        {
        }
    }

    public class InvoiceFieldException : BadRequestException
    {
        public InvoiceFieldException(string fieldName)
            : base($"Invoice with Field {fieldName} is not correct.")
        {
        }
    }
}