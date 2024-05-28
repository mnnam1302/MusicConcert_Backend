using Domain.Abstractions.Aggregates;
using Domain.Abstractions.Entities;
using Domain.Enumerations;
using Domain.Exceptions;

namespace Domain.Entities;

public class Event : AggregateRoot<Guid>, IAuditable, ISoftDeleted
{
    protected Event()
    {   
    }

    private Event(Guid id, string name, DateTimeOffset startDateOnUtc, DateTimeOffset endDateOnUtc, int capacity, Guid organizationId, string eventType)
    {
        Id = id;
        Name = name;
        //StartDate = startDate;
        //StartTime = startTime;
        //EndTime = endTime;
        StartedOnUtc = startDateOnUtc;
        EndedOnUtc = endDateOnUtc;
        Capacity = capacity;
        OrganizationInfoId = organizationId;
        EventType = eventType;
    }

    public static Event Create(
        string name, 
        string? description,
        DateTimeOffset startedDateOutc, DateTimeOffset endedDateOnUtc, 
        int capacity, 
        Guid? categoryId, 
        Guid organizationId,
        string? meetUrl, 
        string? address, 
        string? district, 
        string? city, 
        string? country, 
        Domain.Enumerations.EventType eventType)
    {

        var @event = new Event(Guid.NewGuid(), name, startedDateOutc, endedDateOnUtc, capacity, organizationId, eventType);

        if (!string.IsNullOrWhiteSpace(description))
            @event.AssignDescription(description);

        if (categoryId is not null)
            @event.AssignCategory(categoryId.Value);

        // Business rules
        // Event `online` -> Required MeetUrl
        // Event `offline` -> Required Address, District, City, Country
        if (eventType == Domain.Enumerations.EventType.Online)
        {
            if (string.IsNullOrWhiteSpace(meetUrl))
                throw new EventException.EventTypeException(Domain.Enumerations.EventType.Online);

            @event.AssignEventOnline(eventType, meetUrl);
        }
        else
        {
            if (string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(district) || string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(country))
                throw new EventException.EventTypeException(Domain.Enumerations.EventType.Offline);

            @event.AssignEventOffline(eventType, address, district, city, country);
        }

        return @event;
    }

    //public void Update(string name, string? description, DateTimeOffset startedDateOnUtc, DateTimeOffset endedDateOnUtc, DateTimeOffset? publishedDateOnUtc, Guid? categoryId, string? logoImage, string? layoutImage, string? meetUrl)
    //{

    //}

    private Event AssignEventOnline(EventType eventType, string meetUrl)
    {
        EventType = eventType;
        MeetUrl = meetUrl;
        return this;
    }

    private Event AssignEventOffline(EventType eventType, string address, string district, string city, string country)
    {
        EventType = eventType;
        Address = address;
        District = district;
        City = city;
        Country = country;
        return this;
    }

    private Event AssignCategory(Guid categoryId)
    {
        CategoryId = categoryId;
        return this;
    }

    private Event AssignDescription(string description)
    {
        Description = description;
        return this;
    }

    private Event AssignPublishedEvent(DateTimeOffset publishedOnUtc)
    {
        PublishedOnUtc = publishedOnUtc;
        Status = EventStatus.Published;
        return this;
    }


    // Properties
    public string Name { get; private set; }
    public string? Description { get; private set; }

    // Relationship
    public Guid? CategoryId { get; private set; }
    public virtual Category? Category { get; private set; } = null!;

    public Guid OrganizationInfoId { get; private set; }
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
    // Online -> MeetUrl Required
    // Offline -> Address, District, City, Country Required
    public string EventType { get; private set; } 

    // Link Join Url
    public string? MeetUrl { get; private set; }

    public string Status { get; private set; } = EventStatus.NonPublished;

    public string? Address { get; private set; }
    public string? District { get; private set; }
    public string? City { get; private set; }
    public string? Country { get; private set; }

    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedOnUtc { get; set; }
}