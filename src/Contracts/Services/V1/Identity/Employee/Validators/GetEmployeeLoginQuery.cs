using FluentValidation;

namespace Contracts.Services.V1.Identity.AppEmployee.Validators;

public class GetEmployeeLoginQuery : AbstractValidator<Query.LoginEmployeeQuery>
{
    public GetEmployeeLoginQuery()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(20)
            .Matches("[A-Z]").WithMessage("Password must contain 1 uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain 1 lowercase letter")
            .Matches("[0-9]").WithMessage("Password must contain 1 number");
    }
}