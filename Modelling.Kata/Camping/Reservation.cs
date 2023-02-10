namespace Modelling.Kata.Camping;

public class Reservation
{
    public ReservationId Id { get; }
    public ReservationRequest ReservationRequest { get; }
    public Price BasePrice { get; }
    public Period Period => ReservationRequest.Period;

    public Party MadeBy => ReservationRequest.MadeBy;

    public Reservation(ReservationId id, ReservationRequest reservationRequest, Price basePrice)
    {
        Id = id;
        ReservationRequest = reservationRequest;
        BasePrice = basePrice;
    }
}