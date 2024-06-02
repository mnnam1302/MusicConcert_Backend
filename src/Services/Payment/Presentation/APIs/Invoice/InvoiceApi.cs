using Carter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Presentation.Abstractsions;

namespace Presentation.APIs.Invoice;

public class InvoiceApi : ApiEndpoint, ICarterModule
{
    private readonly string BaseUrl = "/api/v{version:apiVersion}/invoices";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group1 = app.NewVersionedApi("Invoice")
            .MapGroup(BaseUrl).HasApiVersion(1);

        group1.MapPost("", () => "");
        group1.MapGet("", () => "");
        group1.MapGet("{invoiceId}", () => "");
        group1.MapPut("{invoiceId}", () => "");
        group1.MapDelete("{invoiceId}", () => "");
    }
}