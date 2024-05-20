using Contracts.Abstractions.Shared;
using MediatR;

namespace Contracts.Abstractions.Message;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{
}