namespace SeatsSuggestions
{
    /// <summary>
    ///     Occurs when a Suggestion that does not meet expectation is made.
    /// </summary>
    public class SuggestionNotAvailable : SuggestionsMade
    {
        public SuggestionNotAvailable(string showId, int partyRequested) : base(showId, partyRequested)
        {
        }
    }
}