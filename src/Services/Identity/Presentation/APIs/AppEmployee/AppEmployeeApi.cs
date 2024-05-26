using Carter;
using Contracts.Services.V1.Identity.AppEmployee;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Presentation.Abstractions;

namespace Presentation.APIs.AppEmployee;

public class AppEmployeeApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/employee";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group1 = app.NewVersionedApi("Employee")
            .MapGroup(BaseUrl).HasApiVersion(1);

        group1.MapPost("", CreateEmployeesV1);
        group1.MapGet("", () => "");
        group1.MapGet("employeeId", () => "");
        group1.MapPut("employeeId", () => "");
        group1.MapDelete("employeeId", () => "");
    }

    private static async Task<IResult> CreateEmployeesV1(ISender sender, [FromBody] Command.CreateEmployeeCommand command)
    {
        var result = await sender.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }
}