using FluentValidation;

namespace Contracts.Services.V1.Identity.Organization.Validators;

public class CreateOrganizationCommandValidator : AbstractValidator<Command.CreateOrganizationCommand>
{
    public CreateOrganizationCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Industry)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Description)
            .MaximumLength(250);

        RuleFor(x => x.Phone)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.HomePage)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.LogoUrl)
            .MaximumLength(150);

        RuleFor(x => x.Street)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.City)
            .NotEmpty().MaximumLength(30);

        RuleFor(x => x.State)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Country)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.ZipCode)
            .NotEmpty()
            .MaximumLength(20);
    }
}