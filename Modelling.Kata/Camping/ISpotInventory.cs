namespace Modelling.Kata.Camping;

public interface ISpotInventory
{
    bool CanAcceptReservationRequest(ReservationRequest request);
    
    Task<Spot> GetByIdForPeriod(SpotId spotId, Period period);

    Task Save(Spot spot);
}