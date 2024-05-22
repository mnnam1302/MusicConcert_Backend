namespace Domain.Exceptions;

public static class OrganizationInfoException
{
    public class OrganizationNotFoundException : NotFoundException
    {
        public OrganizationNotFoundException(Guid organizationId)
            : base($"The organization info with Id {organizationId} was not found.")
        {
        }
    }
}