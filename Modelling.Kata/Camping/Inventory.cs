namespace Modelling.Kata.Camping;

public class Inventory : IInventory
{
    private readonly ISpotInventory _spotInventory;

    public Inventory(ISpotInventory spotInventory)
    {
        _spotInventory = spotInventory;
    }

    public async Task<Reservation> MakeReservation(ReservationRequest request)
    {
        if (!_spotInventory.CanAcceptReservationRequest(request))
        {
            throw new ArgumentException("Can't accept reservation request", nameof(request));
        }

        var spot = await _spotInventory.GetByIdForPeriod(request.SpotId, request.Period);
        if (spot == null)
        {
            throw new ArgumentNullException($"Spot is unavaialbe for period: {request.Period}");
        }

        var reservationId = ReservationId.New();
        var reservation = new Reservation(reservationId, request, spot.BasePrice);
        spot.Reserve(reservationId, request.Period);
        await _spotInventory.Save(spot);

        return reservation;
    }
}