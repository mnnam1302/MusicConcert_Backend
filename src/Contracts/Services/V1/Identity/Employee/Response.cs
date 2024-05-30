namespace Contracts.Services.V1.Identity.AppEmployee;

public static class Response
{
    public record AuthenticatedResponse
    {
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
}