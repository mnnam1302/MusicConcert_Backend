using AutoMapper;
using Contracts.Abstractions.Message;
using Contracts.Abstractions.Paging;
using Contracts.Abstractions.Shared;
using Contracts.Enumerations;
using Contracts.Services.V1.Catalog.Event;
using Domain.Abstractions.Repositories;
using Domain.Enumerations;
using System.Linq.Expressions;

namespace Application.UseCases.V1.Queries.Event;

public class GetEventsQueryHandler : IQueryHandler<Query.GetEventsQuery, PagedResult<Response.EventResponse>>
{
    private readonly IRepositoryBase<Domain.Entities.Event, Guid> _eventRepository;
    private readonly IMapper _mapper;


    public GetEventsQueryHandler(
        IRepositoryBase<Domain.Entities.Event, Guid> eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<Response.EventResponse>>> Handle(Query.GetEventsQuery request, CancellationToken cancellationToken)
    {
        //1.
        // search - Search City and District
        // filter - StartedDate
        var productQuery = string.IsNullOrEmpty(request.SearchTerm)
            ? _eventRepository.FindAll(x => x.StartedOnUtc >= request.StartedDate && x.Status == EventStatus.Published)
            : _eventRepository.FindAll(x =>
                                    x.City.Contains(request.SearchTerm)
                                    && x.StartedOnUtc >= request.StartedDate
                                    && x.Status == EventStatus.Published);

        //2. sort
        productQuery = request.SortOrder == SortOrder.Descending
            ? productQuery.OrderByDescending(GetSortProperty(request))
            : productQuery.OrderBy(GetSortProperty(request));

        var products = await PagedResult<Domain.Entities.Event>.CreateAsync(
                productQuery, 
                request.PageIndex, 
                request.PageSize, 
                cancellationToken);

        //3. mapping
        var result = _mapper.Map<PagedResult<Response.EventResponse>>(products);
        return Result.Success(result);
    }

    private static Expression<Func<Domain.Entities.Event, object>> GetSortProperty(Query.GetEventsQuery request)
        => request.SortColumn?.ToLower() switch
        {
            "name" => @event => @event.Name,
            "createdonutc" => @event => @event.CreatedOnUtc,
            "publishedonutc" => @event => @event.PublishedOnUtc,
            "startedonutc" => @event => @event.StartedOnUtc,
            _ => @event => @event.CreatedOnUtc

            // _ => product => product.CreatedOnUtc // Default sort Descensing by CreatedOnUtc
        };
}