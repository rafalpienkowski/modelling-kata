namespace Modelling.Kata.Camping;

public record ReservationRequestId(Guid Value)
{
    public static ReservationRequestId NewReservationRequestId => new(Guid.NewGuid());
}