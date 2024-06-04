using FluentValidation;

namespace Contracts.Services.V1.Order.Validators;

public class OrderDetailValidator : AbstractValidator<Command.OrderDetail>
{
    public OrderDetailValidator()
    {
        RuleFor(x => x.TicketId).NotEmpty();

        RuleFor(x => x.UnitPrice).GreaterThan(0);

        RuleFor(x => x.Quantity)
            .InclusiveBetween(1, 5)
            .WithMessage("The quantity must be greater than 0 and less than or equal to 5.");
    }
}