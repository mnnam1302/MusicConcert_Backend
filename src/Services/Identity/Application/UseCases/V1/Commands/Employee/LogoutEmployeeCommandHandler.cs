using Application.Abstractions;
using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Identity.AppEmployee;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using System.Security.Claims;

namespace Application.UseCases.V1.Commands.Employee;

public class LogoutEmployeeCommandHandler : ICommandHandler<Command.LogoutEmployeeCommand>
{
    private readonly ICacheService _cacheService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRepositoryBase<AppEmployee, Guid> _employeeRepository;

    public LogoutEmployeeCommandHandler(ICacheService cacheService, IJwtTokenService jwtTokenService, IRepositoryBase<AppEmployee, Guid> employeeRepository)
    {
        _cacheService = cacheService;
        _jwtTokenService = jwtTokenService;
        _employeeRepository = employeeRepository;
    }

    public async Task<Result> Handle(Command.LogoutEmployeeCommand request, CancellationToken cancellationToken)
    {
        var holderEmployee = await _employeeRepository.FindSingleAsync(x => x.Email.Equals(request.Email), cancellationToken)
            ?? throw new EmployeeException.EmployeeNotFoundByEmailException(request.Email);

        var authenticated = await _cacheService.GetAsync<Response.AuthenticatedResponse>($"session:{request.Email}", cancellationToken)
            ?? throw new IdentityException.TokenException("Can not get value from Redis.");

        var principle = _jwtTokenService.GetPrincipalFromExpiredToken(authenticated.AccessToken);
        var emailKey = principle.FindFirstValue(ClaimTypes.Email).ToString();

        await _cacheService.RemoveAsync($"session:{emailKey}", cancellationToken);

        return Result.Success();
    }
}