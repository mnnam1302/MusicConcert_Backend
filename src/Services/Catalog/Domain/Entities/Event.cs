using Domain.Abstractions.Aggregates;
using Domain.Abstractions.Entities;
using Domain.Enumerations;
using Domain.Exceptions;

namespace Domain.Entities;

public class Event : AggregateRoot<Guid>, IAuditable, ISoftDeleted
{
    protected Event() { }

    private Event(Guid id, string name, DateTimeOffset startDateOnUtc, DateTimeOffset endDateOnUtc, int capacity, string eventType)
    {
        Id = id;
        Name = name;
        StartedOnUtc = startDateOnUtc;
        EndedOnUtc = endDateOnUtc;
        Capacity = capacity;
        EventType = eventType;
    }

    public static Event Create(
        string name, 
        string? description,
        DateTimeOffset startedDateOutc, DateTimeOffset endedDateOnUtc, 
        int capacity, 
        Guid? categoryId, 
        Guid? organizationId,
        string? meetUrl, 
        string? address, 
        string? district, 
        string? city, 
        string? country, 
        Domain.Enumerations.EventType eventType)
    {

        var @event = new Event(Guid.NewGuid(), name, startedDateOutc, endedDateOnUtc, capacity, eventType);

        if (!string.IsNullOrWhiteSpace(description))
            @event.AssignDescription(description);

        if (categoryId is not null)
            @event.AssignCategory(categoryId.Value);

        if (organizationId is not null)
            @event.AssignOrganizationInfo(organizationId.Value);

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

    /// <summary>
    /// This is method aims update Event include: name, description, publishedDateOnUtc, logoImage, layoutImage
    /// before Published Event
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="publishedDateOnUtc"></param>
    /// <param name="logoImage"></param>
    /// <param name="layoutImage"></param>
    public void Update(string? name, string? description, bool publishedDateOnUtc)
    {
        if (!string.IsNullOrWhiteSpace(name))
            AssignName(name);

        if (!string.IsNullOrWhiteSpace(description))
            AssignDescription(description);

        if (publishedDateOnUtc)
            AssignPublishedEvent();
    }

    public void Delete()
    {
        Status = EventStatus.Cancelled;
    }

    public void UpdateLogoImage(string logoImage)
    {
        LogoImage = logoImage;
    }

    public void UpdateLayoutImage(string layoutImage)
    {
        LayoutImage = layoutImage;
    }

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

    private Event AssignOrganizationInfo(Guid organizationId)
    {
        OrganizationInfoId = organizationId;
        return this;
    }

    private Event AssignName(string name)
    {
        Name = name;
        return this;
    }
    
    private Event AssignDescription(string description)
    {
        Description = description;
        return this;
    }

    private Event AssignPublishedEvent()
    {
        PublishedOnUtc = DateTime.UtcNow;
        Status = EventStatus.Published;
        return this;
    }


    // Properties
    public string Name { get; private set; }
    public string? Description { get; private set; }

    // Relationship
    public Guid? CategoryId { get; private set; }
    public virtual Category? Category { get; private set; } = null!;

    public Guid? OrganizationInfoId { get; private set; }
    public virtual OrganizationInfo? OrganizationInfo { get; private set; } = null!;


    // Image
    public string? LogoImage { get; private set; }
    public string? LayoutImage { get; private set; }

    // Event Time
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

    public virtual List<Ticket>? Tickets { get; private set; } = new();
}