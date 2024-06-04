using FluentValidation;

namespace Contracts.Services.V1.Order.Validators;

public class CreateOrderCommandValidator : AbstractValidator<Command.CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("The customer identifier  can't empty.");

        RuleFor(x => x.Details)
            .NotEmpty()
            .WithMessage("The details' order can't empty.");

        RuleForEach(x => x.Details).SetValidator(new OrderDetailValidator());
    }
}