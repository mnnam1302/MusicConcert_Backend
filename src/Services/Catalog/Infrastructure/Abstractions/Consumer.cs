using Contracts.Abstractions.Message;
using MassTransit;
using MediatR;

namespace Infrastructure.Abstractions;

public abstract class Consumer<TMessage> : IConsumer<TMessage>
    where TMessage : class, IDomainEvent
{
    private readonly ISender _sender;

    protected Consumer(ISender sender)
    {
        _sender = sender;
    }

    public async Task Consume(ConsumeContext<TMessage> context)
    {
        await _sender.Send(context.Message);
    }
}