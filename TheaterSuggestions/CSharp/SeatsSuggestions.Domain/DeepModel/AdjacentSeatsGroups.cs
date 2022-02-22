using System.Collections.Generic;

namespace SeatsSuggestions.Domain.DeepModel
{
    public class AdjacentSeatsGroups
    {
        public List<AdjacentSeats> Groups { get; } = new();
    }
}