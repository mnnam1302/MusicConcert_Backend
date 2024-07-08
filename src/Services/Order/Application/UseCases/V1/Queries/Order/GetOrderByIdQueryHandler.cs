using AutoMapper;
using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Order;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;

namespace Application.UseCases.V1.Queries.Order;

public class GetOrderByIdQueryHandler : IQueryHandler<Query.GetOrderByIdQuery, Response.OrderResponse>
{
    private readonly IRepositoryBase<Domain.Entities.Order, Guid> _orderRepository;
    private readonly IMapper _mapper;

    public GetOrderByIdQueryHandler(IRepositoryBase<Domain.Entities.Order, Guid> orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<Result<Response.OrderResponse>> Handle(Query.GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        /*
        * 1. check if order exists
        * 2. map order to response
        */

        // 1.
        var foundOrder = await _orderRepository.FindByIdAsync(request.OrderId, cancellationToken, x => x.OrderDetails)
            ?? throw new NotFoundException("Order was not found.");

        // 2.
        var result = _mapper.Map<Response.OrderResponse>(foundOrder);

        return Result.Success(result);
    }
}