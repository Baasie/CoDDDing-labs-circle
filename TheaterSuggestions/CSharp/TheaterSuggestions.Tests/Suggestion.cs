using System.Collections.Generic;

namespace SeatsSuggestions.Tests
{
    public class Suggestion
    {
        public Suggestion(int partyRequested)
        {
            PartyRequested = partyRequested;
        }

        public int PartyRequested { get; }

        public List<Seat> Seats { get; set; } = new List<Seat>();

        public bool IsFulFilled => Seats.Count == PartyRequested;

        public void AddSeat(Seat seat)
        {
            seat.UpdateCategory(SeatAvailability.Suggested);
            Seats.Add(seat);
        }
    }
}