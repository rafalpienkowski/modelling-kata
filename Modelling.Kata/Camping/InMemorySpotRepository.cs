namespace Modelling.Kata.Camping;

public class InMemorySpotRepository : ISpotRepository
{
    private readonly IList<Spot> _spots = new List<Spot>();

    public Task Save(Spot spot)
    {
        var existingSpot = _spots.FirstOrDefault(s => s.Id == spot.Id);
        if (existingSpot != null)
        {
            if (existingSpot.LastModifiedAt > spot.LastModifiedAt)
            {
                throw new ArgumentException("Spot has been changed by another transaction", nameof(spot));
            }

            _spots.Remove(existingSpot);
        }
        
        _spots.Add(spot);
        
        return Task.CompletedTask;
    }

    public Task<Spot?> GetById(SpotId spotId) => Task.FromResult(_spots.FirstOrDefault(s => s.Id == spotId));
}