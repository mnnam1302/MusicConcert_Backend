using Application.Abstractions;
using Contracts.Abstractions.Message;
using Contracts.Abstractions.Shared;
using Contracts.Services.V1.Identity.Customer;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using System.Security.Claims;

namespace Application.UseCases.V1.Commands.Customer;

public class LogoutCustomerCommandHandler : ICommandHandler<Command.LogoutCustomerCommand>
{
    private readonly ICacheService _cacheService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRepositoryBase<AppCustomer, Guid> _customerRepository;

    public LogoutCustomerCommandHandler(ICacheService cacheService, IJwtTokenService jwtTokenService, IRepositoryBase<AppCustomer, Guid> customerRepository)
    {
        _cacheService = cacheService;
        _jwtTokenService = jwtTokenService;
        _customerRepository = customerRepository;
    }

    public async Task<Result> Handle(Command.LogoutCustomerCommand request, CancellationToken cancellationToken)
    {
        // Step 01: Check email existing?
        var holderCustomer = await _customerRepository.FindSingleAsync(x => x.Email.Equals(request.Email), cancellationToken)
            ?? throw new CustomerException.CustomerNotFoundByEmailException(request.Email);

        // Step 02: Get Redis -> check authenticate
        var authenticated = await _cacheService.GetAsync<Response.AuthenticatedResponse>($"session:{request.Email}", cancellationToken)
           ?? throw new IdentityException.TokenException("Can not get value from Redis.");

        var principle = _jwtTokenService.GetPrincipalFromExpiredToken(authenticated.AccessToken);
        var emailKey = principle.FindFirstValue(ClaimTypes.Email).ToString();

        await _cacheService.RemoveAsync($"session:{emailKey}", cancellationToken);

        return Result.Success();
    }
}