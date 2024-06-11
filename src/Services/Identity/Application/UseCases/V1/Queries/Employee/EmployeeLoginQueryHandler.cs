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
        /*
            1. Check email existing
            2. Check password
            3. Get Claims
            4. Generate Token
            5. Set cache
         */

        //1.
        var holderEmployee = await _employeeRepository.FindSingleAsync(x => x.Email.Equals(request.Email), cancellationToken)
            ?? throw new EmployeeException.EmployeeNotFoundByEmailException(request.Email);

        //2.
        //bool isAuthenticated = _hashPasswordService.VerifyPassword(request.Password, holderEmployee.PasswordHash, holderEmployee.PasswordSalt);

        //if (!isAuthenticated)
        //    throw new IdentityException.AuthenticationException();

        bool isMatch = _hashPasswordService.VerifyPassword(request.Password, holderEmployee.PasswordHash);

        if (!isMatch)
            throw new IdentityException.AuthenticationException();

        //3.
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, holderEmployee.Id.ToString()),
            new (ClaimTypes.Name, holderEmployee.FullName),
            new (ClaimTypes.Email, holderEmployee.Email),
        };

        //4.
        var accessToken = _jwtTokenService.GenerateAccessToken(claims);
        var refershToken = _jwtTokenService.GenerateRefreshToken();

        var result = new Response.AuthenticatedResponse
        {
            AccessToken = accessToken,
            RefreshToken = refershToken,
            RefreshTokenExpiryTime = DateTime.Now.AddMinutes(5)
        };

        //5.
        await _cacheService.SetAsync($"session:{request.Email}", result, cancellationToken);

        return Result.Success(result);
    }
}