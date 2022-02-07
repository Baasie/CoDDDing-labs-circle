using System.Collections.Generic;

namespace SeatsSuggestions.Tests
{
    public class AuditoriumSeating
    {
        public readonly Dictionary<string, Row> Rows;

        public AuditoriumSeating(Dictionary<string, Row> rows)
        {
            Rows = rows;
        }
    }
}