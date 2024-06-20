namespace Contracts.Services.V1.Identity.AppEmployee;

public static class Response
{
    public record AuthenticatedResponse
    {
        public Guid UserId { get; init; }
        public string AccessToken { get; init; }
        public string RefreshToken { get; init; }
        public DateTime? RefreshTokenExpiryTime { get; init; }
    }

    public record EmployeeDetailsResponse(
        Guid Id,
        string FirstName,
        string LastName,
        string FullName,
        string Email,
        string PhoneNumber,
        DateTime? DateOfBirth,
        bool IsDirector,
        bool IsHeadOfDepartment,
        Guid? ManagerId);


    public record EmployeesResponse
    {
        public Guid Id { get; init; }
        public string FullName { get; init; }
        public string Email { get; init; }
        public string PhoneNumber { get; init; }
        public string? OrganizationName { get; init; }
        public bool IsDirector { get; init; }
        public bool IsHeadOfDepartment { get; init; }
        public Guid? ManagerId { get; init; }
    }
}