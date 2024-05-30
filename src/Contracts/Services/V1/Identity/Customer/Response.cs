namespace Contracts.Services.V1.Identity.Customer;

public static class Response
{
    public record AuthenticatedResponse
    {
        public string AccessToken { get; init; }
        public string RefreshToken { get; init; }
        public DateTime? RefreshTokenExpiryTime { get; init; }
    }

    public record CustomerDetailsResponse
    {
        public Guid Id { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string FullName { get; init; }
        public string Email { get; init; }
        public string PhoneNumber { get; init; }
        public DateTime? DateOfBirth { get; init; }
        public string? Address { get; init; }
    }

    public record CustomerResponse
    {
        public Guid Id { get; init; }
        public string FullName { get; init; }
        public string Email { get; init; }
        public string PhoneNumber { get; init; }
        public bool IsDeleted { get; init; }
    }
}