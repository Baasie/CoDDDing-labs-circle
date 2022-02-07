using System.Collections.Generic;

namespace SeatsSuggestions.Tests
{
    /// <summary>
    ///     Occurs when a Suggestion is made.
    /// </summary>
    public class SuggestionMade
    {
        private readonly List<Seat> _suggestedSeats;

        public int PartyRequested { get; }

        public IReadOnlyList<Seat> SuggestedSeats => _suggestedSeats;

        public SuggestionMade(int partyRequested, List<Seat> seats)
        {
            PartyRequested = partyRequested;
            _suggestedSeats = seats;
        }
    }
}