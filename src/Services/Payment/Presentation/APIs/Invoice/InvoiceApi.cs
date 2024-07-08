using Carter;
using Contracts.Services.V1.Payment.Invoice;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        // Query
        //group1.MapGet("", () => "");
        group1.MapGet("customers/{customerId}", GetInvoicesByCustomerIdV1);

        // Command
        group1.MapPut("{invoiceId}/payment", PaymentInvoicesV1);
        group1.MapPut("{invoiceId}/cancel", CancelInvoicesV1);
    }

    private static async Task<IResult> PaymentInvoicesV1(ISender sender,
        Guid invoiceId,
        [FromBody] Command.PaymentInvoiceCommand request)
    {
        var command = new Command.PaymentInvoiceCommand(invoiceId, request.OrderId, request.CustomerId, request.TransactionCode);

        var result = await sender.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    private static async Task<IResult> CancelInvoicesV1(
        ISender sender, 
        Guid invoiceId, 
        [FromBody] Command.CancelInvoiceCommand request)
    {
        var command = new Command.CancelInvoiceCommand(invoiceId, request.OrderId, request.CustomerId);

        var result = await sender.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    private static async Task<IResult> GetInvoicesByCustomerIdV1(
        ISender sender,
        Guid customerId,
        int pageIndex = 1,
        int pageSize = 10)
    {
        var query = new Query.GetInvoicesByCustomerIdQuery(customerId, pageIndex, pageSize);

        var result = await sender.Send(query);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }
}