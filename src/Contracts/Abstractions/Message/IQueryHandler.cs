using Contracts.Abstractions.Shared;
using MediatR;

namespace Contracts.Abstractions.Message;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}