using System.Collections.Generic;

namespace SeatsSuggestions.Domain.DeepModel
{
    public class AdjacentSeats
    {
        public AdjacentSeats()
        {
        }

        public AdjacentSeats(IEnumerable<SeatWithDistance> seatsWithTheDistance)
        {
            SeatsWithDistance.AddRange(seatsWithTheDistance);
        }

        public List<SeatWithDistance> SeatsWithDistance { get; } = new();

        public void AddSeat(SeatWithDistance seatWithDistance)
        {
            SeatsWithDistance.Add(seatWithDistance);
        }

        public override string ToString()
        {
            return string.Join("-", SeatsWithDistance);
        }
    }
}