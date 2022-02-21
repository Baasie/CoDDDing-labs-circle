using System;
using System.Collections.Generic;
using System.Linq;
using SeatsSuggestions.DeepModel;
using Value;

namespace SeatsSuggestions
{
    public class Row : ValueType<Row>
    {
        public Row(string name, IReadOnlyList<Seat> seats)
        {
            Name = name;
            Seats = seats;
        }

        public string Name { get; init; }
        public IReadOnlyList<Seat> Seats { get; init; }

        public Row AddSeat(Seat seat)
        {
            return new Row(Name, new List<Seat>(Seats) { seat });
        }

        public SeatingOptionSuggested SuggestSeatingOption(SuggestionRequest suggestionRequest)
        {
            var seatingOptionSuggested = new SeatingOptionSuggested(suggestionRequest);

            foreach (var seat in OfferAdjacentSeatsNearerTheMiddleOfRow(suggestionRequest))
            {
                seatingOptionSuggested.AddSeat(seat);

                if (seatingOptionSuggested.MatchExpectation()) return seatingOptionSuggested;
            }

            return new SeatingOptionNotAvailable(suggestionRequest);
        }

        public IEnumerable<Seat> OfferAdjacentSeatsNearerTheMiddleOfRow(SuggestionRequest suggestionRequest)
        {
            // 1. offer seats from the middle of the row
            var seatsWithDistanceFromMiddleOfTheRow =
                new OfferingSeatsNearerMiddleOfTheRow(this).OfferSeatsNearerTheMiddleOfTheRow(suggestionRequest);
            
            return seatsWithDistanceFromMiddleOfTheRow.Select(seatWithTheDistanceFromTheMiddleOfTheRow => seatWithTheDistanceFromTheMiddleOfTheRow.Seat);
        }

        public Row Allocate(Seat seat)
        {
            var newVersionOfSeats = new List<Seat>();

            foreach (var currentSeat in Seats)
                if (currentSeat.SameSeatLocation(seat))
                    newVersionOfSeats.Add(new Seat(seat.RowName, seat.Number, seat.PricingCategory,
                        SeatAvailability.Allocated));
                else
                    newVersionOfSeats.Add(currentSeat);

            return new Row(seat.RowName, newVersionOfSeats);
        }

        public int TheMiddleOfRow => Seats.Count % 2 == 0 ? Seats.Count / 2 : Math.Abs(Seats.Count / 2) + 1;

        public bool RowSizeIsEven => Seats.Count % 2 == 0;

        public bool IsMiddleOfTheRow(Seat seat)
        {
            if (RowSizeIsEven)
            {
                if (Math.Abs(seat.Number - TheMiddleOfRow) == 0 || seat.Number - (TheMiddleOfRow + 1) == 0)
                {
                    return true;
                }
            }

            return Math.Abs(seat.Number - TheMiddleOfRow) == 0;
        }

        public int DistanceFromTheMiddleOfRow(Seat seat)
        {
            if (RowSizeIsEven)
                return seat.Number - TheMiddleOfRow > 0
                    ? Math.Abs((int)(seat.Number - TheMiddleOfRow))
                    : Math.Abs((int)(seat.Number - (TheMiddleOfRow + 1)));

            return Math.Abs((int)(seat.Number - TheMiddleOfRow));
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] { Name, new ListByValue<Seat>(new List<Seat>(Seats)) };
        }
    }
}