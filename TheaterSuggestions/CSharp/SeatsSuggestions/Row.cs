using System.Collections.Generic;

namespace SeatsSuggestions
{
    public class Row
    {
        public string Name { get; }
        public List<Seat> Seats { get; }

        public Row(string name, List<Seat> seats)
        {
            Name = name;
            Seats = seats;
        }

        public void AddSeat(Seat seat)
        {
            Seats.Add(seat);
        }

        public SeatingOptionSuggested SuggestSeatingOption(int partyRequested, PricingCategory pricingCategory)
        {
            foreach (var seat in Seats)
            {
                if (seat.IsAvailable() && seat.MatchCategory(pricingCategory))
                {
                    var seatAllocation = new SeatingOptionSuggested(partyRequested, pricingCategory);

                    seatAllocation.AddSeat(seat);

                    if (seatAllocation.MatchExpectation())
                    {
                        return seatAllocation;
                    }
                }
            }

            return new SeatingOptionNotAvailable(partyRequested, pricingCategory);
        }
    }
}