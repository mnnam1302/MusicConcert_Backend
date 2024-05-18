namespace Domain.Abstractions.Primitives;

public abstract class ValueObject : IEquatable<ValueObject>
{
    public abstract IEnumerable<object> GetAtomicValues();

    public static bool operator ==(ValueObject? a, ValueObject? b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(ValueObject? a, ValueObject? b)
        => !(a == b);

    public bool Equals(ValueObject? other)
        => other is not null && ValuesAreEqual(other);

    public override bool Equals(object? obj)
        => obj is ValueObject other && ValuesAreEqual(other);

    public override int GetHashCode() =>
        GetAtomicValues()
            .Aggregate(
                default(int),
                HashCode.Combine);

    private bool ValuesAreEqual(ValueObject other)
        => GetAtomicValues().SequenceEqual(other.GetAtomicValues());
}