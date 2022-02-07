using System.Collections.Generic;

namespace SeatsSuggestions
{
    public class SeatingOptionSuggested
    {
        public PricingCategory PricingCategory { get; }
        public List<Seat> Seats { get; } = new List<Seat>();
        public int PartyRequested { get; }

        public SeatingOptionSuggested(int partyRequested, PricingCategory priceCategory)
        {
            PartyRequested = partyRequested;
            PricingCategory = priceCategory;
        }

        public void AddSeat(Seat seat)
        {
            Seats.Add(seat);
        }

        public bool MatchExpectation()
        {
            return Seats.Count == PartyRequested;
        }
    }
}