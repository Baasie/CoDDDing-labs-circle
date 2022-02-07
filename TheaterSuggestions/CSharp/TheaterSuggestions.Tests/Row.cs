using System.Collections.Generic;

namespace SeatsSuggestions.Tests
{
    public class Row
    {
        public string Name { get; set; }
        public List<Seat> Seats { get; set; } = new List<Seat>();
    }
}