using Carter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Presentation.APIs.AppCustomer;

public class AppCustomerApi : ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/customer";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        //var group1 = app.NewVersionedApi("Customer")
        //    .MapGroup(BaseUrl).HasApiVersion(1);

        //group1.MapPost("", () => "");
    }
}