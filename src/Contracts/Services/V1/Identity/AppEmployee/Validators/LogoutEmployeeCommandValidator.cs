using FluentValidation;

namespace Contracts.Services.V1.Identity.AppEmployee.Validators;

public class LogoutEmployeeCommandValidator : AbstractValidator<Command.LogoutEmployeeCommand>
{
    public LogoutEmployeeCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.AccessToken)
            .NotEmpty();
    }
}