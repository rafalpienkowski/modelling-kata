namespace Modelling.Kata.Camping;

public class SpotInventory : ISpotInventory
{
    private readonly ISpotRepository _spotRepository;

    public SpotInventory(ISpotRepository spotRepository)
    {
        _spotRepository = spotRepository;
    }

    public bool CanAcceptReservationRequest(ReservationRequest request)
    {
        //Hard-coded policy
        //In the future we can add them or load
        return request.Period.NumberOfDays >= 3;
    }

    public async Task<Spot> GetByIdForPeriod(SpotId spotId, Period period)
    {
        var spot = await _spotRepository.GetById(spotId);
        if (spot == null)
        {
            throw new ArgumentNullException($"There is no spot with id: {spotId.Value}");
        }

        if (spot.GetReservationStatus(period) == ReservationStatus.Reserved)
        {
            throw new ArgumentOutOfRangeException(nameof(period), $"Period: {period} is not available");
        }

        return spot;
    }

    public Task Save(Spot spot) => _spotRepository.Save(spot);
}