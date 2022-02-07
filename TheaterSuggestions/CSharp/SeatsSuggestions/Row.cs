using System.Collections.Generic;
using System.Linq;
using Value;

namespace SeatsSuggestions
{
    public class Row : ValueType<Row>
    {
        public string Name { get; init; }
        public List<Seat> Seats { get; init; }

        public Row(string name, List<Seat> seats)
        {
            Name = name;
            Seats = seats;
        }

        public Row AddSeat(Seat seat)
        {
            return new Row(Name, new List<Seat>(Seats) { seat }); ;
        }

        public SeatingOptionSuggested SuggestSeatingOption(int partyRequested, PricingCategory pricingCategory)
        {
            foreach (var seat in Seats)
            {
                if (seat.IsAvailable() && seat.MatchCategory(pricingCategory))
                {
                    var seatingOptionSuggested = new SeatingOptionSuggested(partyRequested, pricingCategory);

                    seatingOptionSuggested.AddSeat(seat);

                    if (seatingOptionSuggested.MatchExpectation())
                    {
                        return seatingOptionSuggested;
                    }
                }
            }

            return new SeatingOptionNotAvailable(partyRequested, pricingCategory);
        }

        public Row Allocate(Seat seat)
        {
            var newVersionOfSeats = new List<Seat>();

            foreach (var currentSeat in Seats)
            {
                if (currentSeat.SameSeatLocation(seat))
                {
                    newVersionOfSeats.Add(new Seat(seat.RowName, seat.Number, seat.PricingCategory,
                        SeatAvailability.Allocated));
                }
                else
                {
                    newVersionOfSeats.Add(currentSeat);
                }
            }

            return new Row(seat.RowName, newVersionOfSeats);
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] {Name, new ListByValue<Seat>(Seats)};
        }
    }
}