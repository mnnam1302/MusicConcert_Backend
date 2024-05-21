using Contracts.Abstractions.Message;
using Contracts.Services.V1.Identity;
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
                    case nameof(DomainEvent.OrganizationCreated):
                        var productCreated = JsonConvert.DeserializeObject<DomainEvent.OrganizationCreated>(
                            outboxMessage.Content,
                            new JsonSerializerSettings
                            {
                                TypeNameHandling = TypeNameHandling.All
                            });

                        await _publishEndpoint.Publish(productCreated, context.CancellationToken);
                        break;

                    case nameof(DomainEvent.OrganizationUpdated):
                        var productUpdated = JsonConvert.DeserializeObject<DomainEvent.OrganizationUpdated>(
                            outboxMessage.Content,
                            new JsonSerializerSettings {
                                TypeNameHandling = TypeNameHandling.All
                            });

                        await _publishEndpoint.Publish(productUpdated, context.CancellationToken);
                        break;

                    case nameof(DomainEvent.OrganizationDeleted):
                        var productDeleted = JsonConvert.DeserializeObject<DomainEvent.OrganizationDeleted>(
                            outboxMessage.Content,
                            new JsonSerializerSettings {
                                TypeNameHandling = TypeNameHandling.All
                            });

                        await _publishEndpoint.Publish(productDeleted, context.CancellationToken);
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