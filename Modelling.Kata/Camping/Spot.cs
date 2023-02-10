namespace Modelling.Kata.Camping;

public class Spot
{
    public SpotId Id { get; }

    private IList<SpotReservation> SpotReservations { get; }
    public Price BasePrice { get; }

    public DateTimeOffset LastModifiedAt { get; private set; }

    public Spot(SpotId id, Price basePrice, IList<SpotReservation> spotReservations, DateTimeOffset lastModifiedAt)
    {
        Id = id;
        BasePrice = basePrice;
        SpotReservations = spotReservations;
        LastModifiedAt = lastModifiedAt;
    }

    public void Reserve(ReservationId reservationId, Period period)
    {
        if (SpotReservations.Any(sr => sr.Period.OverlapsWith(period)))
        {
            throw new ArgumentOutOfRangeException($"Spot is alredy reserved for period: {period}");
        }
        
        SpotReservations.Add(new SpotReservation(reservationId, period));
        LastModifiedAt = DateTimeOffset.UtcNow;
    }

    public void Cancel(ReservationId reservationId)
    {
        var spotReservation = SpotReservations.FirstOrDefault(sr => sr.ReservationId == reservationId);
        if (spotReservation == null)
        {
            return;
        }

        SpotReservations.Remove(spotReservation);
        LastModifiedAt = DateTimeOffset.UtcNow;
    }

    public ReservationStatus GetReservationStatus(Period period) =>
        SpotReservations.Any(sr => sr.Period.OverlapsWith(period))
            ? ReservationStatus.Reserved
            : ReservationStatus.Available;
}