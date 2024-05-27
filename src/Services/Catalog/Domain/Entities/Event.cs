using Domain.Abstractions.Aggregates;
using Domain.Abstractions.Entities;
using Domain.Enumerations;

namespace Domain.Entities;

public class Event : AggregateRoot<Guid>, IAuditable, ISoftDeleted
{
    protected Event()
    {   
    }

    //public Event(Guid id, string name, DateTimeOffset startDateOnUtc, DateTimeOffset endDateOnUtc, int capacity, string eventType, string status)
    //{
    //    Id = id;
    //    Name = name;
    //    //StartDate = startDate;
    //    //StartTime = startTime;
    //    //EndTime = endTime;
    //    StartedOnUtc = startDateOnUtc;
    //    EndedOnUtc = endDateOnUtc;
    //    Capacity = capacity;
    //    EventType = eventType;
    //    Status = status;
    //}

    public string Name { get; private set; }
    public string? Description { get; private set; }

    // Relationship
    public Guid? CategoryId { get; private set; }
    public virtual Category? Category { get; private set; } = null!;

    public Guid? OrganizationInfoId { get; private set; }
    //public OrganizationInfo? OrganizationInfo { get; private set; } = null!;


    // Image
    public string? LogoImage { get; private set; }
    public string? LayoutImage { get; private set; }

    // Event Time
    //public DateOnly StartDate { get; private set; }
    //public TimeOnly StartTime { get; private set; }
    //public TimeOnly EndTime { get; private set; }

    public DateTimeOffset StartedOnUtc { get; private set; }
    public DateTimeOffset EndedOnUtc { get; private set; }

    public DateTimeOffset? PublishedOnUtc { get; private set; }

    public int Capacity { get; private set; }
    
    // EventType: Online | Offline
    public string EventType { get; private set; }

    // Link Join Url
    public string? MeetUrl { get; private set; }

    // Status
    public string Status { get; private set; }

    // Address
    public string? Address { get; private set; }
    public string? District { get; private set; }
    public string? City { get; private set; }
    public string? Country { get; private set; }


    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedOnUtc { get; set; }
}