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

    public record OrganizationResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Phone { get; init; }
        public string HomePage { get; init; }
        public string City { get; init; }
        public string Country { get; init; }
    }
}