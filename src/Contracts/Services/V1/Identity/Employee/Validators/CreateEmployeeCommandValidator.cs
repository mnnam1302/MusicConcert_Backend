using FluentValidation;

namespace Contracts.Services.V1.Identity.AppEmployee.Validators;

public class CreateEmployeeCommandValidator : AbstractValidator<Command.CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(request => request.FirstName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(20);

        RuleFor(request => request.LastName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(20);

        RuleFor(request => request.PhoneNumber)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(request => request.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(request => request.Password)
            .NotEmpty()
            .MinimumLength(6)
            .MaximumLength(20)
            .Matches("[A-Z]").WithMessage("Password must contain 1 uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain 1 lowercase letter")
            .Matches("[0-9]").WithMessage("Password must contain 1 number");

        RuleFor(request => request.PasswordConfirmation)
            .Equal(request => request.Password)
            .WithMessage("{PropertyName} must match {ComparisonProperty}");
    }
}