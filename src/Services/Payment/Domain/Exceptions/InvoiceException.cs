namespace Domain.Exceptions;

public static class InvoiceException
{
    public class InvoiceNotFoundException : NotFoundException
    {
        public InvoiceNotFoundException(Guid invoiceId)
            : base($"Invoice with Id {invoiceId} was not found.")
        {
            
        }
    }
}