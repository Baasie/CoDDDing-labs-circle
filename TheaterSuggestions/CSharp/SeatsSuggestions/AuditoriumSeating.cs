using System.Collections.Generic;

namespace SeatsSuggestions
{
    public class AuditoriumSeating
    {
        public IReadOnlyDictionary<string, Row> Rows => _rows;

        private readonly Dictionary<string, Row> _rows;

        public AuditoriumSeating(Dictionary<string, Row> rows)
        {
            _rows = rows;
        }

        public SeatingOptionSuggested SuggestSeatingOptionFor(int partyRequested, PricingCategory pricingCategory)
        {
            foreach (var row in _rows.Values)
            {
                var seatOptionsSuggested = row.SuggestSeatingOption(partyRequested, pricingCategory);

                if (seatOptionsSuggested.MatchExpectation())
                {
                    return seatOptionsSuggested;
                }
            }

            return new SeatingOptionNotAvailable(partyRequested, pricingCategory);
        }
    }
}