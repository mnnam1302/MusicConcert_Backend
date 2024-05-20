using Contracts.Abstractions.Shared;
using MediatR;

namespace Contracts.Abstractions.Message;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
