namespace SilentMike.DietMenu.Core.Domain.Types;

public abstract class StronglyTypedValue<T> : IEquatable<StronglyTypedValue<T>> where T : IComparable<T>
{
    public T Value { get; }

    public StronglyTypedValue(T value)
        => this.Value = value;

    public bool Equals(StronglyTypedValue<T>? other)
    {
        if (other is null)
        {
            return false;
        }

        return object.ReferenceEquals(this, other) || EqualityComparer<T>.Default.Equals(this.Value, other.Value);
    }

    public override bool Equals(object? @object)
    {
        if (@object is null)
        {
            return false;
        }

        if (object.ReferenceEquals(this, @object))
        {
            return true;
        }

        if (@object.GetType() != this.GetType())
        {
            return false;
        }

        return this.Equals((StronglyTypedValue<T>)@object);
    }

    public override int GetHashCode()
        => EqualityComparer<T>.Default.GetHashCode(this.Value);

    public static bool operator ==(StronglyTypedValue<T>? left, StronglyTypedValue<T>? right)
        => object.Equals(left, right);

    public static bool operator !=(StronglyTypedValue<T>? left, StronglyTypedValue<T>? right)
        => !object.Equals(left, right);
}
