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
                    // DomainEvent: PaymentProcessed and PaymentProcessedFailed
                    case nameof (DomainEvent.PaymentProcessed):
                        var paymentProcessed = JsonConvert.DeserializeObject<DomainEvent.PaymentProcessed>(
                            outboxMessage.Content,
                             new JsonSerializerSettings
                             {
                                 TypeNameHandling = TypeNameHandling.All
                             });

                        await _publishEndpoint.Publish(paymentProcessed, context =>
                            context.CorrelationId = context.Message.OrderId);

                        break;

                    case nameof (DomainEvent.PaymentProcessedFailed):
                        var paymentProcessedFailed = JsonConvert.DeserializeObject<DomainEvent.PaymentProcessedFailed>(outboxMessage.Content,
                            new JsonSerializerSettings
                                                        {
                                 TypeNameHandling = TypeNameHandling.All
                             });

                        await _publishEndpoint.Publish(paymentProcessedFailed, context =>
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