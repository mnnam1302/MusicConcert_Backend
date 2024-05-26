using Application.Abstractions;
using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Identity.AppEmployee;
using Domain.Exceptions;
using System.Security.Claims;

namespace Application.UseCases.V1.Commands.AppEmployee;

public class LogoutEmployeeCommandHandler : ICommandHandler<Command.LogoutEmployeeCommand>
{
    private readonly ICacheService _cacheService;
    private readonly IJwtTokenService _jwtTokenService;

    public LogoutEmployeeCommandHandler(ICacheService cacheService, IJwtTokenService jwtTokenService)
    {
        _cacheService = cacheService;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result> Handle(Command.LogoutEmployeeCommand request, CancellationToken cancellationToken)
    {
        var authenticated = await _cacheService.GetAsync<Response.AuthenticateResponse>($"session:{request.Email}", cancellationToken)
            ?? throw new IdentityException.TokenException("Can not get value from Redis.");

        var principle = _jwtTokenService.GetPrincipalFromExpiredToken(authenticated.AccessToken);
        var emailKey = principle.FindFirstValue(ClaimTypes.Email).ToString();

        await _cacheService.RemoveAsync($"session:{emailKey}", cancellationToken);

        return Result.Success();
    }
}