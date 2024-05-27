﻿using Application.Abstractions;
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
        // Step 01: Check cache
        var authenticated = await _cacheService.GetAsync<Response.AuthenticatedResponse>($"session:{request.Email}", cancellationToken)
            ?? throw new IdentityException.TokenException("Can not get value from Redis.");

        // Step 02: Verify access token
        var principles = _jwtTokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        var emailKey = principles.FindFirstValue(ClaimTypes.Email).ToString();

        // Step 03: Check refresh token
        if (authenticated.RefreshToken != request.RefreshToken || authenticated.RefreshTokenExpiryTime <= DateTime.Now)
            throw new IdentityException.TokenException("Request token invalid!");

        // Step 04: Generate new token
        var accessToken = _jwtTokenService.GenerateAccessToken(principles.Claims);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();

        // Step 05: Update cache
        var result = new Response.AuthenticatedResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            RefreshTokenExpiryTime = DateTime.Now.AddMinutes(5)
        };

        await _cacheService.SetAsync($"session:{emailKey}", result, cancellationToken);
        return Result.Success(result);
    }
}