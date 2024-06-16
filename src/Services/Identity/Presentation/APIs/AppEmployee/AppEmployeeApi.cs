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
    private const string BaseUrl = "/api/v{version:apiVersion}/employees";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group1 = app.NewVersionedApi("Employee")
            .MapGroup(BaseUrl).HasApiVersion(1);

        group1.MapPost("", CreateEmployeesV1);
        //group1.MapPut("employeeId", () => "");
        group1.MapDelete("{employeeId}", DeleteEmployeesV1);

        // QUERY //
        //group1.MapGet("", () => "");
        group1.MapGet("{employeeId}", GetEmployeesByIdV1);
        group1.MapGet("", GetEmployeesForOrganizationV1);
    }

    // QUERY //
    private static async Task<IResult> GetEmployeesByIdV1(ISender sender, Guid employeeId)
    {
        var query = new Query.GetEmployeeByIdQuery(employeeId);
        var result = await sender.Send(query);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    private static async Task<IResult> CreateEmployeesV1(ISender sender, [FromBody] Command.CreateEmployeeCommand command)
    {
        var result = await sender.Send(command);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    private static async Task<IResult> GetEmployeesForOrganizationV1(
        ISender sender,
        Guid? organizationId = null,
        int pageIndex = 1,
        int pageSize = 10)
    {
        var query = new Contracts.Services.V1.Identity.AppEmployee.Query.GetEmployeesByOrganizationQuery(
            organizationId,
            pageIndex,
            pageSize);
        var result = await sender.Send(query);

        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }

    // END QUERY //

    private static async Task<IResult> DeleteEmployeesV1(ISender sender, Guid employeeId)
    {
        var command = new Command.DeleteEmployeeCommand(employeeId);

        var result = await sender.Send(command);

        if (result.IsFailure)
            HandlerFailure(result);

        return Results.Ok(result);
    }
}