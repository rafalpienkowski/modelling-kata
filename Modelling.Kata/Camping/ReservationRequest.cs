namespace Modelling.Kata.Camping;

public record ReservationRequest(ReservationRequestId Id, SpotId SpotId, Period Period,
    Party MadeBy, DateTimeOffset ReceivedAt);