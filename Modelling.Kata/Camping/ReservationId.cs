namespace Modelling.Kata.Camping;

public record ReservationId
{
    public Guid Value { get; }

    private ReservationId(Guid value)
    {
        Value = value;
    }

    public static ReservationId New() => new(Guid.NewGuid());
}