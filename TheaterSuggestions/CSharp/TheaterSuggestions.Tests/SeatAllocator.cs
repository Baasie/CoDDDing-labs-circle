namespace SeatsSuggestions.Tests
{
    public class SeatAllocator
    {
        private readonly AuditoriumSeatingAdapter _auditoriumSeatingAdapter;

        public SeatAllocator(AuditoriumSeatingAdapter auditoriumSeatingAdapter)
        {
            _auditoriumSeatingAdapter = auditoriumSeatingAdapter;
        }

        public SuggestionMade MakeSuggestion(string showId, int partyRequested)
        {
            var suggestion = new Suggestion(partyRequested);

            var auditoriumSeating = _auditoriumSeatingAdapter.GetAuditoriumSeating(showId);

            foreach (var row in auditoriumSeating.Rows)
            foreach (var seat in row.Value.Seats)
            {
                if (seat.IsAvailable)
                {
                    suggestion.AddSeat(seat);

                    if (suggestion.IsFulFilled)
                    {
                        return new SuggestionMade(suggestion.PartyRequested, suggestion.Seats);
                    }
                }
            }

            return new SuggestionNotAvailable(partyRequested);
        }
    }
}