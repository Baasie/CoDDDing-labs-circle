using System.Collections.Generic;
using Value;

namespace SeatsSuggestions.DeepModel
{
    /// <summary>
    ///     Our model uses a seat with a property DistanceFromTheMiddle
    ///     to manage these business rules:
    ///     * Offer seats nearer middle of the row.
    ///     * Offer adjacent seats to member of the same party.
    /// </summary>
    public class SeatWithDistance : ValueType<SeatWithDistance>
    {
        public SeatWithDistance(Seat seat, int distanceFromTheMiddleOfTheRow)
        {
            Seat = seat;
            DistanceFromTheMiddleOfTheRow = distanceFromTheMiddleOfTheRow;
        }

        public Seat Seat { get; }
        public int DistanceFromTheMiddleOfTheRow { get; }

        public override string ToString()
        {
            return $"{Seat.RowName}{Seat.Number} {DistanceFromTheMiddleOfTheRow}";
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] { Seat, DistanceFromTheMiddleOfTheRow };
        }
    }
}