namespace Domain.Exceptions;

public static class OrganizationInfoException
{
    public class OrganizationNotFoundException : NotFoundException
    {
        public OrganizationNotFoundException(Guid organizationId)
            : base($"Organization info with Id {organizationId} was not found.")
        {
        }
    }

    public class OrganizaitonInfoAlreadyExistsException : Exception
    {
        public OrganizaitonInfoAlreadyExistsException(Guid organizationId)
            : base($"Organizaiton info with Id {organizationId} already exists.")
        {
        }
    }
}