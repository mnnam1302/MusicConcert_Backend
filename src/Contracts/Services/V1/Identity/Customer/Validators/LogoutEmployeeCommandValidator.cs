using FluentValidation;

namespace Contracts.Services.V1.Identity.Customer.Validators;

public class LogoutEmployeeCommandValidator : AbstractValidator<Command.LogoutCustomerCommand>
{
    public LogoutEmployeeCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}