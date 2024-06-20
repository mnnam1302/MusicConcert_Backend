using Application.Abstractions;
using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Identity.Customer;
using Domain.Exceptions;
using System.Security.Claims;

namespace Application.UseCases.V1.Queries.Customer;

public class CustomerRefreshTokenQueryHandler : IQueryHandler<Query.CustomerRefreshTokenQuery, Response.AuthenticatedResponse>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ICacheService _cacheService;

    public CustomerRefreshTokenQueryHandler(IJwtTokenService jwtTokenService, ICacheService cacheService)
    {
        _jwtTokenService = jwtTokenService;
        _cacheService = cacheService;
    }

    public async Task<Result<Response.AuthenticatedResponse>> Handle(Query.CustomerRefreshTokenQuery request, CancellationToken cancellationToken)
    {
        /*
            1. check email => Get cache by email
            2. verify access token -> check email input and redis
            3. check refresh token input and redis
            4. generate new token
            5. update cache
         */

        // 1.
        var authenticated = await _cacheService.GetAsync<Response.AuthenticatedResponse>($"session:{request.Email}", cancellationToken)
            ?? throw new IdentityException.TokenException("Can not get value from Redis.");

        // 2.
        var principles = _jwtTokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        var emailKey = principles.FindFirstValue(ClaimTypes.Email).ToString();

        if (emailKey != request.Email)
            throw new IdentityException.TokenException("Request token invalid!");

        // 3.
        if (authenticated.RefreshToken != request.RefreshToken || authenticated.RefreshTokenExpiryTime <= DateTime.Now)
            throw new IdentityException.TokenException("Request token invalid!");

        // 4.
        var accessToken = _jwtTokenService.GenerateAccessToken(principles.Claims);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();

        // 5.
        var result = new Response.AuthenticatedResponse
        {
            UserId = authenticated.UserId,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            RefreshTokenExpiryTime = DateTime.Now.AddDays(1)
        };

        await _cacheService.SetAsync($"session:{emailKey}", result, cancellationToken);
        return Result.Success(result);
    }
}