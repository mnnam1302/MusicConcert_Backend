using Contracts.Abstractions.Message;
using Contracts.Services.V1.Catalog.Ticket;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Persistence;
using Persistence.OutboxMessages;
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

        foreach (OutboxMessage outboxMessage in messages)
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
                    // DomainEvent: Ticket
                    case nameof(DomainEvent.TicketCreated):
                        var ticketCreated = JsonConvert.DeserializeObject<DomainEvent.TicketCreated>(
                            outboxMessage.Content,
                            new JsonSerializerSettings
                            {
                                TypeNameHandling = TypeNameHandling.All
                            });

                        await _publishEndpoint.Publish(ticketCreated, context.CancellationToken);
                        break;

                    case nameof(DomainEvent.TicketDeleted):
                        var ticketDeleted = JsonConvert.DeserializeObject<DomainEvent.TicketDeleted>(
                            outboxMessage.Content,
                            new JsonSerializerSettings
                            {
                                TypeNameHandling = TypeNameHandling.All
                            });

                        await _publishEndpoint.Publish(ticketDeleted, context.CancellationToken);
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