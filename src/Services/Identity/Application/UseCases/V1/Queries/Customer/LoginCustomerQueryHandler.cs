﻿using Application.Abstractions;
using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Identity.Customer;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using System.Security.Claims;

namespace Application.UseCases.V1.Queries.Customer;

public class LoginCustomerQueryHandler : IQueryHandler<Query.LoginCustomerQuery, Response.AuthenticatedResponse>
{
    private readonly IRepositoryBase<AppCustomer, Guid> _customerRepository;
    private readonly IHashPasswordService _hashPasswordService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ICacheService _cacheService;

    public LoginCustomerQueryHandler(IRepositoryBase<AppCustomer, Guid> customerRepository, IHashPasswordService hashPasswordService, IJwtTokenService jwtTokenService, ICacheService cacheService)
    {
        _customerRepository = customerRepository;
        _hashPasswordService = hashPasswordService;
        _jwtTokenService = jwtTokenService;
        _cacheService = cacheService;
    }

    public async Task<Result<Response.AuthenticatedResponse>> Handle(Query.LoginCustomerQuery request, CancellationToken cancellationToken)
    {
        // Step 01: Check email customer
        var holderCustomer = await _customerRepository.FindSingleAsync(x => x.Email.Equals(request.Email), cancellationToken)
            ?? throw new CustomerException.CustomerNotFoundByEmailException(request.Email);

        // Step 02: Check password
        bool isAuthenticated = _hashPasswordService.VerifyPassword(request.Password, holderCustomer.PasswordHash, holderCustomer.PasswordSalt);

        if (!isAuthenticated)
            throw new IdentityException.AuthenticationException();

        // Step 03: Get Claims
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, holderCustomer.Id.ToString()),
            new (ClaimTypes.Name, holderCustomer.FullName),
            new (ClaimTypes.Email, holderCustomer.Email),
        };

        // Step 04: Generate Token
        var accessToken = _jwtTokenService.GenerateAccessToken(claims);
        var refershToken = _jwtTokenService.GenerateRefreshToken();

        var result = new Response.AuthenticatedResponse
        {
            AccessToken = accessToken,
            RefreshToken = refershToken,
            RefreshTokenExpiryTime = DateTime.Now.AddMinutes(5)
        };

        // Step 05: Set cache
        await _cacheService.SetAsync($"session:{request.Email}", result, cancellationToken);

        return Result.Success(result);
    }
}