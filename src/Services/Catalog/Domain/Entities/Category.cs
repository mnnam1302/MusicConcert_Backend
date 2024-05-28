using Domain.Abstractions.Aggregates;
using Domain.Abstractions.Entities;

namespace Domain.Entities;

public class Category : AggregateRoot<Guid>, ISoftDeleted
{
    protected Category() { }

    private Category(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public static Category Create(Guid id, string name, string? description)
    {
        var category = new Category(id, name);

        if (!string.IsNullOrEmpty(description))
            category.AssignDescription(description);

        return category;
    }

    public void Update(string name, string? description)
    {
        Name = name;

        if (!string.IsNullOrEmpty(description))
            AssignDescription(description);
    }

    private Category AssignDescription(string description)
    {
        Description = description;
        return this;
    }

    public string Name { get; private set; }
    public string? Description { get; private set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset? DeletedOnUtc { get; set; }
}