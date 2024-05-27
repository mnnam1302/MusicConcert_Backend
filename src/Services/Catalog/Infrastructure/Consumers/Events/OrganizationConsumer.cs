using Contracts.Services.V1.Identity.Organization;
using Infrastructure.Abstractions;
using MediatR;

namespace Infrastructure.Consumers.Events;

public static class OrganizationConsumer
{
    public class OrganizationCreatedConsumer : Consumer<DomainEvent.OrganizationCreated>
    {
        public OrganizationCreatedConsumer(ISender sender)
            : base(sender)
        {
        }
    }

    //public class OrganizationUpdatedConsumer : Consumer<DomainEvent.OrganizationUpdated>
    //{
    //    public OrganizationUpdatedConsumer(ISender sender)
    //        : base(sender)
    //    {
    //    }
    //}

    public class OrganizationDeletedConsumer : Consumer<DomainEvent.OrganizationDeleted>
    {
        public OrganizationDeletedConsumer(ISender sender)
            : base(sender)
        {
        }
    }
}