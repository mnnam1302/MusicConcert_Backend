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
    private readonly IRepositoryBase<Domain.Entities.OrderDetails, Guid> _orderDetailRepository;
    private readonly IMapper _mapper;

    public GetOrderByIdQueryHandler(
        IRepositoryBase<Domain.Entities.Order, Guid> orderRepository,
        IRepositoryBase<Domain.Entities.OrderDetails, Guid> orderDetailRepository,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _orderDetailRepository = orderDetailRepository;
        _mapper = mapper;
    }

    public async Task<Result<Response.OrderResponse>> Handle(Query.GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        /*
        * 1. check if order exists => Order
        * 2. get order details and ticketInfo
        * 2. map order to response
        */

        // 1.
        var foundOrder = await _orderRepository.FindByIdAsync(request.OrderId, cancellationToken)
            ?? throw new NotFoundException("Order was not found.");

        // 2.
        var orderDetails = await _orderDetailRepository.FindAllAsync(x => 
                                                                        x.OrderId == foundOrder.Id, 
                                                                        cancellationToken, 
                                                                        x => x.TicketInfo);

        // 3.
        //var result = _mapper.Map<Response.OrderResponse>(foundOrder);
        var result = new Response.OrderResponse
        {
            Id = foundOrder.Id,
            Status = foundOrder.Status,
            CreatedOnUtc = foundOrder.CreatedOnUtc,
            OrderDetails = _mapper.Map<List<Response.OrderDetailsResponse>>(orderDetails)
            //OrderDetails = new()
        };

        return Result.Success(result);
    }
}