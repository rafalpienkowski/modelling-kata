namespace Modelling.Kata.Camping;

public interface IInventory
{
    Task<Reservation> MakeReservation(ReservationRequest request);
}