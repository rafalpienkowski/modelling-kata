using Modelling.Kata.Camping;

namespace Modelling.Kata.Tests.Camping;

public class InventoryShould
{
    [Fact]
    public async Task Made_Reservation_For_Available_Spot()
    {
        var spotId = new SpotId(Guid.NewGuid());
        var basePrice = new Price(new Money(Currency.Pln, 200), Period.BasedOn(new DateTime(2000, 1, 1), 100));
        var someSpot = new Spot(spotId, basePrice, new List<SpotReservation>(), DateTimeOffset.UtcNow);
        var spotRepository = new InMemorySpotRepository();
        await spotRepository.Save(someSpot);
        
        var spotInventory = new SpotInventory(spotRepository);
        var inventory = new Inventory(spotInventory);
        
        var reservationRequestId = ReservationRequestId.NewReservationRequestId;
        var period = Period.BasedOn(new DateTime(2000, 1, 2), 3);
        var madeBy = new Party("John Rambo");
        var reservationRequest = new ReservationRequest(reservationRequestId, spotId, period, madeBy,
            DateTimeOffset.UtcNow);

        var reservation = await inventory.MakeReservation(reservationRequest);

        reservation.Should().NotBeNull();
        reservation.ReservationRequest.Should().Be(reservationRequest);
        reservation.Period.Should().Be(period);
        reservation.BasePrice.Should().Be(basePrice);
        reservation.MadeBy.Should().Be(madeBy);
    }

    [Fact]
    public async Task Made_Two_Not_Overlapping_Reservations_For_The_Same_Spot()
    {
        var spotId = new SpotId(Guid.NewGuid());
        var basePrice = new Price(new Money(Currency.Pln, 200), Period.BasedOn(new DateTime(2000, 1, 1), 100));
        var someSpot = new Spot(spotId, basePrice, new List<SpotReservation>(), DateTimeOffset.UtcNow);
        var spotRepository = new InMemorySpotRepository();
        await spotRepository.Save(someSpot);
        
        var spotInventory = new SpotInventory(spotRepository);
        var inventory = new Inventory(spotInventory);
        
        var reservationRequestId = ReservationRequestId.NewReservationRequestId;
        var period = Period.BasedOn(new DateTime(2000, 1, 2), 3);
        var johnRambo = new Party("John Rambo");
        var reservationRequest = new ReservationRequest(reservationRequestId, spotId, period, johnRambo,
            DateTimeOffset.UtcNow);

        var reservation = await inventory.MakeReservation(reservationRequest);

        reservation.Should().NotBeNull();
        reservation.ReservationRequest.Should().Be(reservationRequest);
        reservation.Period.Should().Be(period);
        reservation.BasePrice.Should().Be(basePrice);
        reservation.MadeBy.Should().Be(johnRambo);
        
        var secondReservationRequestId = ReservationRequestId.NewReservationRequestId;
        var notOverlappingPeriod = Period.BasedOn(new DateTime(2000, 2, 2), 3);
        var mickyMouse = new Party("Micky Mouse");
        var secondReservationRequest = new ReservationRequest(secondReservationRequestId, spotId, notOverlappingPeriod, mickyMouse,
            DateTimeOffset.UtcNow);

        var secondReservation = await inventory.MakeReservation(secondReservationRequest);
        
        secondReservation.Should().NotBeNull();
        secondReservation.ReservationRequest.Should().Be(secondReservationRequest);
        secondReservation.Period.Should().Be(notOverlappingPeriod);
        secondReservation.BasePrice.Should().Be(basePrice);
        secondReservation.MadeBy.Should().Be(mickyMouse);
    }

    [Fact]
    public async Task Made_Two_Overlapping_Reservations_For_Two_Different_Spots()
    {
        var spotId = new SpotId(Guid.NewGuid());
        var anotherSpotId = new SpotId(Guid.NewGuid());
        var basePrice = new Price(new Money(Currency.Pln, 200), Period.BasedOn(new DateTime(2000, 1, 1), 100));
        var someSpot = new Spot(spotId, basePrice, new List<SpotReservation>(), DateTimeOffset.UtcNow);
        
        var anotherSpot = new Spot(anotherSpotId, basePrice, new List<SpotReservation>(), DateTimeOffset.UtcNow);
        var spotRepository = new InMemorySpotRepository();
        await spotRepository.Save(someSpot);
        await spotRepository.Save(anotherSpot);
        
        var spotInventory = new SpotInventory(spotRepository);
        var inventory = new Inventory(spotInventory);
        
        var reservationRequestId = ReservationRequestId.NewReservationRequestId;
        var period = Period.BasedOn(new DateTime(2000, 1, 2), 3);
        var johnRambo = new Party("John Rambo");
        var reservationRequest = new ReservationRequest(reservationRequestId, spotId, period, johnRambo,
            DateTimeOffset.UtcNow);

        var reservation = await inventory.MakeReservation(reservationRequest);

        reservation.Should().NotBeNull();
        reservation.ReservationRequest.Should().Be(reservationRequest);
        reservation.Period.Should().Be(period);
        reservation.BasePrice.Should().Be(basePrice);
        reservation.MadeBy.Should().Be(johnRambo);
        
        var secondReservationRequestId = ReservationRequestId.NewReservationRequestId;
        var mickyMouse = new Party("Micky Mouse");
        var secondReservationRequest = new ReservationRequest(secondReservationRequestId, anotherSpotId, period,
            mickyMouse, DateTimeOffset.UtcNow);

        var secondReservation = await inventory.MakeReservation(secondReservationRequest);
        
        secondReservation.Should().NotBeNull();
        secondReservation.ReservationRequest.Should().Be(secondReservationRequest);
        secondReservation.Period.Should().Be(period);
        secondReservation.BasePrice.Should().Be(basePrice);
        secondReservation.MadeBy.Should().Be(mickyMouse);
    }

    [Fact]
    public async Task Block_An_Reservation_With_Overlapping_Period()
    {
        var spotId = new SpotId(Guid.NewGuid());
        var basePrice = new Price(new Money(Currency.Pln, 200), Period.BasedOn(new DateTime(2000, 1, 1), 100));
        var someSpot = new Spot(spotId, basePrice, new List<SpotReservation>(), DateTimeOffset.UtcNow);
        
        var spotRepository = new InMemorySpotRepository();
        await spotRepository.Save(someSpot);
        
        var spotInventory = new SpotInventory(spotRepository);
        var inventory = new Inventory(spotInventory);
        
        var reservationRequestId = ReservationRequestId.NewReservationRequestId;
        var period = Period.BasedOn(new DateTime(2000, 1, 2), 3);
        var johnRambo = new Party("John Rambo");
        var reservationRequest = new ReservationRequest(reservationRequestId, spotId, period, johnRambo,
            DateTimeOffset.UtcNow);

        var reservation = await inventory.MakeReservation(reservationRequest);

        reservation.Should().NotBeNull();
        
        var secondReservationRequestId = ReservationRequestId.NewReservationRequestId;
        var mickyMouse = new Party("Micky Mouse");
        var overlappingPeriod = Period.BasedOn(new DateTime(2000, 1, 3), 5);
        var secondReservationRequest = new ReservationRequest(secondReservationRequestId, spotId, overlappingPeriod,
            mickyMouse, DateTimeOffset.UtcNow);

        var action = () => inventory.MakeReservation(secondReservationRequest);

        await action.Should().ThrowAsync<ArgumentOutOfRangeException>();
    }
}