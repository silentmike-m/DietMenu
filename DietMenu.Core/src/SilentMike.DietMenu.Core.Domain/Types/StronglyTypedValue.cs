namespace SilentMike.DietMenu.Core.Domain.Types;

public abstract class StronglyTypedValue<T> : IEquatable<StronglyTypedValue<T>>
    where T : IComparable<T>
{
    private T Value { get; }

    protected StronglyTypedValue(T value)
        => this.Value = value;

    public bool Equals(StronglyTypedValue<T>? other)
    {
        if (other is null)
        {
            return false;
        }

        return object.ReferenceEquals(this, other) || EqualityComparer<T>.Default.Equals(this.Value, other.Value);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (object.ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != this.GetType())
        {
            return false;
        }

        return this.Equals((StronglyTypedValue<T>)obj);
    }

    public override int GetHashCode()
        => EqualityComparer<T>.Default.GetHashCode(this.Value);

    public static bool operator ==(StronglyTypedValue<T>? left, StronglyTypedValue<T>? right)
        => object.Equals(left, right);

    public static bool operator !=(StronglyTypedValue<T>? left, StronglyTypedValue<T>? right)
        => !object.Equals(left, right);
}
