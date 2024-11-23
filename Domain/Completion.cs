namespace Domain
{
    public class Completion : IComparable

    {
    public DateTime When { get; set; }
    public int Occurrences { get; set; }

    public Completion(DateTime when, int occurrences)
    {
        When = when;
        Occurrences = occurrences;
    }

    public override bool Equals(object? obj)
    {
        return obj is Completion completion
               && When == completion.When;
    }

    public int CompareTo(object? obj)
    {
        if (obj is not Completion other)
            throw new ArgumentException();

        return this.When.CompareTo(other.When);
    }

    public static bool operator ==(Completion left, Completion right) =>
        left.Equals(right);

    public static bool operator !=(Completion left, Completion right) =>
        !left.Equals(right);
    };
}
