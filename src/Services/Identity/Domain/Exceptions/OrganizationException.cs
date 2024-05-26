namespace Domain.Exceptions;

public static class OrganizationException
{
    public class OrganizationNotFoundException : NotFoundException
    {
        public OrganizationNotFoundException(Guid organizaitonId)
            : base($"The organization with ID {organizaitonId} was not found.")
        {    
        }
    }
}