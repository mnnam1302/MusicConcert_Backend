using Contracts.Abstractions.Message;
using Contracts.Services.V1.Identity.Organization;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Persistence;
using Persistence.Outbox;
using Quartz;

namespace Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProducerOutboxMessageJob : IJob
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;

    public ProducerOutboxMessageJob(ApplicationDbContext dbContext, IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        List<OutboxMessage> messages = await _dbContext.Set<OutboxMessage>()
            .Where(x => x.ProcessedOnUtc == null)
            .OrderBy(x => x.OccurredOnUtc)
            .Take(20)
            .ToListAsync(context.CancellationToken);

        foreach(OutboxMessage outboxMessage in messages)
        {
            IDomainEvent? domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
                    outboxMessage.Content,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });

            if (domainEvent == null)
                continue;

            try
            {
                switch (domainEvent.GetType().Name)
                {
                    // DomainEvent: Organization
                    case nameof(DomainEvent.OrganizationCreated):
                        var organizationCreated = JsonConvert.DeserializeObject<DomainEvent.OrganizationCreated>(
                            outboxMessage.Content,
                            new JsonSerializerSettings
                            {
                                TypeNameHandling = TypeNameHandling.All
                            });

                        await _publishEndpoint.Publish(organizationCreated, context.CancellationToken);
                        break;

                    // Stop: event organization-updated
                    //case nameof(DomainEvent.OrganizationUpdated):
                    //    var organizationUpdated = JsonConvert.DeserializeObject<DomainEvent.OrganizationUpdated>(
                    //        outboxMessage.Content,
                    //        new JsonSerializerSettings {
                    //            TypeNameHandling = TypeNameHandling.All
                    //        });

                    //    await _publishEndpoint.Publish(organizationUpdated, context.CancellationToken);
                    //    break;

                    case nameof(DomainEvent.OrganizationDeleted):
                        var organizationDeleted = JsonConvert.DeserializeObject<DomainEvent.OrganizationDeleted>(
                            outboxMessage.Content,
                            new JsonSerializerSettings {
                                TypeNameHandling = TypeNameHandling.All
                            });

                        await _publishEndpoint.Publish(organizationDeleted, context.CancellationToken);
                        break;

                    // DomainEvent: Employee
                    //case nameof(Contracts.Services.V1.Identity.AppEmployee.DomainEvent.EmployeeCreated):
                    //    var employeeCreated = JsonConvert.DeserializeObject<Contracts.Services.V1.Identity.AppEmployee.DomainEvent.EmployeeCreated>(
                    //        outboxMessage.Content,
                    //        new JsonSerializerSettings
                    //        {
                    //            TypeNameHandling = TypeNameHandling.All
                    //        });

                    //    await _publishEndpoint.Publish(employeeCreated, context.CancellationToken);
                    //    break;

                    //case nameof(Contracts.Services.V1.Identity.AppEmployee.DomainEvent.EmployeeDeleted):
                    //    var employeeDeleted = JsonConvert.DeserializeObject<Contracts.Services.V1.Identity.AppEmployee.DomainEvent.EmployeeDeleted>(
                    //        outboxMessage.Content,
                    //        new JsonSerializerSettings
                    //        {
                    //            TypeNameHandling = TypeNameHandling.All
                    //        });

                    //    await _publishEndpoint.Publish(employeeDeleted, context.CancellationToken);
                    //    break;

                    // DomainEvent: Customer
                    case nameof(Contracts.Services.V1.Identity.Customer.DomainEvent.CustomerCreated):
                        var customerCreated = JsonConvert.DeserializeObject<Contracts.Services.V1.Identity.Customer.DomainEvent.CustomerCreated>(
                            outboxMessage.Content,
                            new JsonSerializerSettings
                            {
                                TypeNameHandling = TypeNameHandling.All
                            });

                        await _publishEndpoint.Publish(customerCreated, context.CancellationToken);
                        break;

                    case nameof(Contracts.Services.V1.Identity.Customer.DomainEvent.CustomerUpdated):
                        var customerUpdated = JsonConvert.DeserializeObject<Contracts.Services.V1.Identity.Customer.DomainEvent.CustomerUpdated>(
                            outboxMessage.Content,
                            new JsonSerializerSettings
                            {
                                TypeNameHandling = TypeNameHandling.All
                            });

                        await _publishEndpoint.Publish(customerUpdated, context.CancellationToken);
                        break;

                    case nameof(Contracts.Services.V1.Identity.Customer.DomainEvent.CustomerDeleted):
                        var customerDeleted = JsonConvert.DeserializeObject<Contracts.Services.V1.Identity.Customer.DomainEvent.CustomerDeleted>(
                            outboxMessage.Content,
                            new JsonSerializerSettings
                            {
                                TypeNameHandling = TypeNameHandling.All
                            });

                        await _publishEndpoint.Publish(customerDeleted, context.CancellationToken);
                        break;

                    default:
                        break;
                }

                outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                outboxMessage.Error = ex.Message;
            }
        }

        await _dbContext.SaveChangesAsync(context.CancellationToken);
    }
}