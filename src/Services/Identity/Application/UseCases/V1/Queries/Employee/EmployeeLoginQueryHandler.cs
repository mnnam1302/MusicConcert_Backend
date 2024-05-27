using Application.Abstractions;
using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Identity.AppEmployee;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;
using System.Security.Claims;

namespace Application.UseCases.V1.Queries.Employee;

public class EmployeeLoginQueryHandler : IQueryHandler<Query.LoginEmployeeQuery, Response.AuthenticatedResponse>
{
    private readonly IRepositoryBase<Domain.Entities.AppEmployee, Guid> _employeeRepository;
    private readonly IHashPasswordService _hashPasswordService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ICacheService _cacheService;

    public EmployeeLoginQueryHandler(IRepositoryBase<Domain.Entities.AppEmployee, Guid> employeeRepository, IHashPasswordService hashPasswordService, IJwtTokenService jwtTokenService, ICacheService cacheService)
    {
        _employeeRepository = employeeRepository;
        _hashPasswordService = hashPasswordService;
        _jwtTokenService = jwtTokenService;
        _cacheService = cacheService;
    }

    public async Task<Result<Response.AuthenticatedResponse>> Handle(Query.LoginEmployeeQuery request, CancellationToken cancellationToken)
    {
        // Step 01: Check email existing?
        var holderEmployee = await _employeeRepository.FindSingleAsync(x => x.Email.Equals(request.Email), cancellationToken)
            ?? throw new EmployeeException.EmployeeNotFoundByEmailException(request.Email);

        // Step 02: Check password
        bool isAuthenticated = _hashPasswordService.VerifyPassword(request.Password, holderEmployee.PasswordHash, holderEmployee.PasswordSalt);

        if (!isAuthenticated)
            throw new IdentityException.AuthenticationException();

        // Step 03: Get Claims
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, holderEmployee.Id.ToString()),
            new (ClaimTypes.Name, holderEmployee.FullName),
            new (ClaimTypes.Email, holderEmployee.Email),
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