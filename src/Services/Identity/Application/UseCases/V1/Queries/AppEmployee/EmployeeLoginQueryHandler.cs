using Application.Abstractions;
using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Identity.AppEmployee;
using Domain.Abstractions.Repositories;
using Domain.Exceptions;
using System.Security.Claims;

namespace Application.UseCases.V1.Queries.AppEmployee;

public class EmployeeLoginQueryHandler : IQueryHandler<Query.EmployeeLoginQuery, Response.AuthenticateResponse>
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

    public async Task<Result<Response.AuthenticateResponse>> Handle(Query.EmployeeLoginQuery request, CancellationToken cancellationToken)
    {
        // Check email existing?
        var holderEmployee = await _employeeRepository.FindSingleAsync(x => x.Email.Equals(request.Email), cancellationToken)
            ?? throw new EmployeeException.EmployeeNotFoundByEmailException(request.Email);

        // Check password
        bool isAuthenticated = _hashPasswordService.VerifyPassword(request.Password, holderEmployee.PasswordHash, holderEmployee.PasswordSalt);

        if (!isAuthenticated)
            throw new IdentityException.AuthenticationException();

        // Get Claims
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, holderEmployee.Id.ToString()),
            new (ClaimTypes.Name, holderEmployee.FullName),
            new (ClaimTypes.Email, holderEmployee.Email),
        };

        // Get roles

        // Generate Token
        var accessToken = _jwtTokenService.GenerateAccessToken(claims);
        var refershToken = _jwtTokenService.GenerateRefreshToken();

        var result = new Response.AuthenticateResponse
        {
            AccessToken = accessToken,
            RefreshToken = refershToken,
            RefreshTokenExpiryTime = DateTime.Now.AddMinutes(5)
        };

        // Set cache
        await _cacheService.SetAsync($"session:{request.Email}", result, cancellationToken);

        // Return result 
        return Result.Success(result);
    }
}