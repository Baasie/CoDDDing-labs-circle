using System.Collections.Generic;

namespace SeatsSuggestions
{
    public class SeatingOptionSuggested
    {
        public PricingCategory PricingCategory { get; }
        public List<Seat> Seats { get; } = new List<Seat>();
        public int PartyRequested { get; }

        public SeatingOptionSuggested(SuggestionRequest suggestionRequest)
        {
            PartyRequested = suggestionRequest.PartyRequested;
            PricingCategory = suggestionRequest.PricingCategory;
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