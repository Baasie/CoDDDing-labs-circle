using System.Collections.Generic;

namespace SeatsSuggestions.Domain
{
    public class SeatingOptionSuggested
    {
        public SeatingOptionSuggested(SuggestionRequest suggestionRequest)
        {
            PartyRequested = suggestionRequest.PartyRequested;
            PricingCategory = suggestionRequest.PricingCategory;
        }

        public PricingCategory PricingCategory { get; }
        public List<Seat> Seats { get; } = new();
        public PartyRequested PartyRequested { get; }

        public bool MatchExpectation()
        {
            return Seats.Count == PartyRequested.PartySize;
        }

        public void AddSeat(Seat seat)
        {
            Seats.Add(seat);
        }
    }
}