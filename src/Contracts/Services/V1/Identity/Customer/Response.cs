﻿namespace Contracts.Services.V1.Identity.Customer;

public static class Response
{
    public record AuthenticatedResponse
    {
        public string AccessToken { get; init; }
        public string RefreshToken { get; init; }
        public DateTime? RefreshTokenExpiryTime { get; init; }
    }
}