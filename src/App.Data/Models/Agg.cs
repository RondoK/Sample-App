using System.ComponentModel.DataAnnotations;

namespace App.Data.Models;

public class Agg : IEquatable<Agg>
{
    [Key]
    public int Id { get; set; }
    public string? Text { get; set; }

    public bool Equals(Agg? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Agg)obj);
    }

    public override int GetHashCode()
    {
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        return Id;
    }

    public static bool operator ==(Agg? left, Agg? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Agg? left, Agg? right)
    {
        return !Equals(left, right);
    }
}