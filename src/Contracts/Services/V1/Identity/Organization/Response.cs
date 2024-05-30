namespace Contracts.Services.V1.Identity.Organization;

public static class Response
{
    public record OrganizationDetailsResponse(
        Guid Id,
        string Name,
        string? Description,
        string Phone,
        string HomePage,
        string? LogoUrl,
        string Street,
        string City,
        string State,
        string Country,
        string ZipCode);

    public record OrganizationResponse(
        Guid Id,
        string Name,
        string Phone,
        string HomePage,
        string City,
        string Country);
}