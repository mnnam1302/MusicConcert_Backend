using Contracts.Enumerations;
using FluentValidation;

namespace Contracts.Services.V1.Catalog.Event.Validators;

public class CreateEventCommandValidator : AbstractValidator<Command.CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        // StartedDateOnUtc must be greater than or equal to the current date and time.
        RuleFor(x => x.StartedDateOnUtc)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateTimeOffset.UtcNow);

        // EndedDateOnUtc must be greater than or equal to StartedDateOnUtc.
        RuleFor(x => x.EndedDateOnUtc)
            .NotEmpty()
            .GreaterThanOrEqualTo(x => x.StartedDateOnUtc);

        RuleFor(x => x.Capacity)
            .GreaterThan(0);

        RuleFor(x => x.OrganizationId)
            .NotEmpty();


        // Rule for EventType must be Online and MeetUrl must not be null or empty
        // or EventType must be Offline and Adrress, District, City, and Country must not be null or empty.
        // I'd like to MeetUrl í https./...
        RuleFor(x => x.EventType)
            .NotEmpty()
            .Must(x => x.Equals(EventType.Online) || x.Equals(EventType.Offline));

        When(x => x.Equals(EventType.Online), () =>
        {
            RuleFor(x => x.MeetUrl)
                .NotEmpty()
                .Matches(@"^(http|https)://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(/\S*)?$");
        });

        When(x => x.Equals(EventType.Offline), () =>
        {
            RuleFor(x => x.Adrress)
                .NotEmpty();

            RuleFor(x => x.District)
                .NotEmpty();

            RuleFor(x => x.City)
                .NotEmpty();

            RuleFor(x => x.Country)
                .NotEmpty();
        });
    }
}