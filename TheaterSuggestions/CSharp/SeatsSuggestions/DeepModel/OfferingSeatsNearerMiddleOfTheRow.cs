using System;
using System.Collections.Generic;
using System.Linq;
using Value;

namespace SeatsSuggestions.DeepModel
{
    /// <summary>
    ///     Business Rule: offer seats nearer the middle of a row
    /// </summary>
    public class OfferingSeatsNearerMiddleOfTheRow : ValueType<OfferingSeatsNearerMiddleOfTheRow>
    {
        private readonly Row _row;

        public OfferingSeatsNearerMiddleOfTheRow(Row row)
        {
            _row = row;
        }

        public IEnumerable<SeatWithTheDistanceFromTheMiddleOfTheRow> OfferSeatsNearerTheMiddleOfTheRow(
            SuggestionRequest suggestionRequest)
        {
            return ComputeDistancesNearerTheMiddleOfTheRow()
                .Where(seatWithTheDistanceFromTheMiddleOfTheRow =>
                    seatWithTheDistanceFromTheMiddleOfTheRow.Seat.MatchCategory(suggestionRequest.PricingCategory))
                .Where(seatWithTheDistanceFromTheMiddleOfTheRow => seatWithTheDistanceFromTheMiddleOfTheRow.Seat.IsAvailable());
        }

        private IEnumerable<SeatWithTheDistanceFromTheMiddleOfTheRow> ComputeDistancesNearerTheMiddleOfTheRow()
        {
            var seatsWithDistancesFromTheMiddle = SplitSeatsByDistanceNearerTheMiddleOfTheRow(seat => _row.IsMiddleOfTheRow(seat));

            return seatsWithDistancesFromTheMiddle.Append(GetSeatsInTheMiddleOfTheRow())
                .SelectMany(seatWithDistanceFromTheMiddleOfTheRows => seatWithDistanceFromTheMiddleOfTheRows)
                .OrderBy(s => s.DistanceFromTheMiddleOfTheRow);
        }

        private IEnumerable<SeatWithTheDistanceFromTheMiddleOfTheRow> GetSeatsInTheMiddleOfTheRow()
        {
            return GetSeatsInTheMiddleOfTheRow(_row.Seats, _row.TheMiddleOfRow)
                .Select(s => new SeatWithTheDistanceFromTheMiddleOfTheRow(s, 0));
        }

        private static IEnumerable<Seat> GetSeatsInTheMiddleOfTheRow(IReadOnlyList<Seat> seats, int middle)
        {
            return seats.Count % 2 == 0
                ? new List<Seat> { seats[middle - 1], seats[middle] }
                : new List<Seat> { seats[middle - 1] };
        }

        private IEnumerable<List<SeatWithTheDistanceFromTheMiddleOfTheRow>> SplitSeatsByDistanceNearerTheMiddleOfTheRow(
            Func<Seat, bool> predicate)
        {
            var seatWithDistances = new List<SeatWithTheDistanceFromTheMiddleOfTheRow>();
            
            foreach (var seat in _row.Seats)
                if (!predicate(seat))
                {
                    seatWithDistances.Add(
                        new SeatWithTheDistanceFromTheMiddleOfTheRow(seat, _row.DistanceFromTheMiddleOfRow(seat)));
                }
                else
                {
                    if (seatWithDistances.Count > 0)
                        yield return seatWithDistances;
                    seatWithDistances = new List<SeatWithTheDistanceFromTheMiddleOfTheRow>();
                }

            if (seatWithDistances.Count > 0)
                yield return seatWithDistances;
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] { _row };
        }
    }
}