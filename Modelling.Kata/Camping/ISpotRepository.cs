namespace Modelling.Kata.Camping;

public interface ISpotRepository
{
    Task Save(Spot spot);
    Task<Spot?> GetById(SpotId spotId);
}