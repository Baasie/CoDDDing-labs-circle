using System.Collections.Generic;
using Value;
using Value.Shared;

namespace SeatsSuggestions
{
    public class AuditoriumSeating : ValueType<AuditoriumSeating>
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
                var seatingOption = row.SuggestSeatingOption(partyRequested, pricingCategory);

                if (seatingOption.MatchExpectation())
                {
                    return seatingOption;
                }
            }

            return new SeatingOptionNotAvailable(partyRequested, pricingCategory);
        }

        public AuditoriumSeating Allocate(SeatingOptionSuggested seatingOptionSuggested)
        {
            // Update the seat references in the Auditorium
            var newAuditorium = AllocateSeats(seatingOptionSuggested.Seats);

            return newAuditorium;
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] {new DictionaryByValue<string, Row>(_rows)};
        }

        private AuditoriumSeating AllocateSeats(IEnumerable<Seat> updatedSeats)
        {
            var newVersionOfRows = new Dictionary<string, Row>(_rows);

            foreach (var updatedSeat in updatedSeats)
            {
                var formerRow = newVersionOfRows[updatedSeat.RowName];
                var newVersionOfRow = formerRow.Allocate(updatedSeat);
                newVersionOfRows[updatedSeat.RowName] = newVersionOfRow;
            }

            return new AuditoriumSeating(newVersionOfRows);
        }
    }
}