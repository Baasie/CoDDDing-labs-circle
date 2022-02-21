using System.Collections.Generic;
using System.Linq;

namespace SeatsSuggestions
{
    /// <summary>
    ///     Occurs when a Suggestion is made.
    /// </summary>
    public class SuggestionMade
    {
        private readonly List<Seat> _suggestedSeats;
        public int PartyRequested { get; }
        public PricingCategory PricingCategory { get; }

        public IReadOnlyList<Seat> SuggestedSeats => _suggestedSeats;

        public SuggestionMade(int partyRequested, PricingCategory pricingCategory, List<Seat> seats)
        {
            PartyRequested = partyRequested;
            PricingCategory = pricingCategory;
            _suggestedSeats = seats.OrderBy(s => s.Number).ToList(); ;
        }

        public IEnumerable<string> SeatNames()
        {
            return _suggestedSeats.Select(s => s.ToString());
        }

        public bool MatchExpectation()
        {
            return _suggestedSeats.Count == PartyRequested;
        }
    }
}