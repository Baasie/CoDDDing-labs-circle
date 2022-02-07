using System.Collections.Generic;

namespace SeatsSuggestions.Tests
{
    /// <summary>
    ///     Occurs when a Suggestion that does not meet expectation is made.
    /// </summary>
    public class SuggestionNotAvailable : SuggestionMade
    {
        public SuggestionNotAvailable(int partyRequested) : base(partyRequested, new List<Seat>())
        {
        }
    }
}