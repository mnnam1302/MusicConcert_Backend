using Contracts.Abstractions.Message;
using Contracts.Services.V1.Order;
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
                    // DomainEvent: Order at Order Service
                    case (nameof(DomainEvent.OrderCreated)):
                        var orderCreated = JsonConvert.DeserializeObject<DomainEvent.OrderCreated>(
                            outboxMessage.Content,
                            new JsonSerializerSettings
                            {
                                TypeNameHandling = TypeNameHandling.All
                            });

                        await _publishEndpoint.Publish(orderCreated, context => 
                            context.CorrelationId = context.Message.OrderId);

                        break;

                    case (nameof(DomainEvent.OrderValidated)):
                        var orderValidated = JsonConvert.DeserializeObject<DomainEvent.OrderValidated>(
                            outboxMessage.Content,
                            new JsonSerializerSettings
                            {
                                TypeNameHandling = TypeNameHandling.All
                            });

                        await _publishEndpoint.Publish(orderValidated, context =>
                            context.CorrelationId = context.Message.OrderId);

                        break;

                    case (nameof(DomainEvent.OrderCancelled)):
                        var orderCancelled = JsonConvert.DeserializeObject<DomainEvent.OrderCancelled>(
                            outboxMessage.Content,
                            new JsonSerializerSettings
                            {
                                TypeNameHandling = TypeNameHandling.All
                            });

                        await _publishEndpoint.Publish(orderCancelled, context =>
                            context.CorrelationId = context.Message.OrderId);
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