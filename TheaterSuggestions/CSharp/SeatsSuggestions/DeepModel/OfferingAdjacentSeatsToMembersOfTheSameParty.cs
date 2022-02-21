using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SeatsSuggestions.DeepModel
{
    /// <summary>
    ///     Business Rule: offer adjacent seats to members of the same party
    /// </summary>
    public static class OfferingAdjacentSeatsToMembersOfTheSameParty
    {
        private static AdjacentSeats NoAdjacentSeatFound { get; } = new(new List<SeatWithDistance>());

        public static IEnumerable<Seat>
            OfferAdjacentSeats(SuggestionRequest suggestionRequest,
                IEnumerable<SeatWithDistance> seatsWithDistanceFromTheMiddleOfRow)
        {
            var adjacentSeatsGroups =
                SplitInGroupsOfAdjacentSeats(suggestionRequest, seatsWithDistanceFromTheMiddleOfRow);

            return SelectAdjacentSeatsWithShorterDistanceFromTheMiddleOfTheRow(suggestionRequest, adjacentSeatsGroups)
                .AdaptAdjacentSeats();
        }


        private static IEnumerable<Seat>
            AdaptAdjacentSeats(this AdjacentSeats adjacentSeats)
        {
            return adjacentSeats.SeatsWithDistance.Select(s => s.Seat);
        }

        private static AdjacentSeats
            SelectAdjacentSeatsWithShorterDistanceFromTheMiddleOfTheRow(SuggestionRequest suggestionRequest,
                AdjacentSeatsGroups groupOfAdjacentSeats)
        {
            var theBestDistancesNearerTheMiddleOfTheRowPerGroup = new SortedDictionary<int, AdjacentSeatsGroups>();

            // To select the best group of adjacent seats, we sort them by their distances
            foreach (var adjacentSeats in groupOfAdjacentSeats.Groups)
            {
                if (!IsMatchingPartyRequested(suggestionRequest, adjacentSeats.SeatsWithDistance)) continue;

                var sumOfDistances = SumOfDistancesNearerTheMiddleOfTheRowPerSeat(adjacentSeats);

                if (!theBestDistancesNearerTheMiddleOfTheRowPerGroup.ContainsKey(sumOfDistances))
                    theBestDistancesNearerTheMiddleOfTheRowPerGroup[sumOfDistances] = new AdjacentSeatsGroups();

                theBestDistancesNearerTheMiddleOfTheRowPerGroup[sumOfDistances].Groups.Add(adjacentSeats);
            }

            return theBestDistancesNearerTheMiddleOfTheRowPerGroup.Any()
                ? SelectTheBestGroup(theBestDistancesNearerTheMiddleOfTheRowPerGroup)
                : NoAdjacentSeatFound;
        }

        private static int
            SumOfDistancesNearerTheMiddleOfTheRowPerSeat(AdjacentSeats adjacentSeats)
        {
            return adjacentSeats.SeatsWithDistance.Sum(s => s.DistanceFromTheMiddleOfTheRow);
        }

        private static AdjacentSeatsGroups
            SortGroupsByDistanceFromMiddleOfTheRow(SuggestionRequest suggestionRequest,
                AdjacentSeatsGroups groupOfAdjacentSeats)
        {
            var adjacentSeatsGroups = new AdjacentSeatsGroups();

            foreach (var seats in groupOfAdjacentSeats.Groups.Select(adjacentSeats =>
                adjacentSeats.SeatsWithDistance.OrderBy(s => s.DistanceFromTheMiddleOfTheRow)))
                adjacentSeatsGroups.Groups.Add(
                    new AdjacentSeats(seats.Take(suggestionRequest.PartyRequested)));

            return adjacentSeatsGroups;
        }

        private static AdjacentSeats
            SelectTheBestGroup(SortedDictionary<int, AdjacentSeatsGroups> bestGroups)
        {
            return HasOnlyOneBestGroup(bestGroups)
                ? ProjectToAdjacentSeats(bestGroups)
                : DecideWhichGroupIsTheBestWhenDistancesAreEqual(bestGroups);
        }

        private static AdjacentSeats
            DecideWhichGroupIsTheBestWhenDistancesAreEqual(SortedDictionary<int, AdjacentSeatsGroups> bestGroups)
        {
            SortedDictionary<int, AdjacentSeats> decideBetweenIdenticalScores = new();

            foreach (var adjacentSeatsGroups in bestGroups.Values)
                SelectTheBestScoreBetweenGroups(decideBetweenIdenticalScores, adjacentSeatsGroups);

            return decideBetweenIdenticalScores.LastOrDefault().Value;
        }

        private static void
            SelectTheBestScoreBetweenGroups(SortedDictionary<int, AdjacentSeats> decideBetweenIdenticalScores,
                AdjacentSeatsGroups adjacentSeatsGroups)
        {
            foreach (var adjacentSeats in adjacentSeatsGroups.Groups)
                if (!decideBetweenIdenticalScores.ContainsKey(adjacentSeats.SeatsWithDistance.Count))
                    decideBetweenIdenticalScores[adjacentSeats.SeatsWithDistance.Count] = adjacentSeats;
        }


        private static AdjacentSeats
            ProjectToAdjacentSeats(SortedDictionary<int, AdjacentSeatsGroups> bestGroups)
        {
            return FirstValues(bestGroups).Groups[0];
        }

        private static AdjacentSeatsGroups
            FirstValues(SortedDictionary<int, AdjacentSeatsGroups> bestGroups)
        {
            return bestGroups.Values.First();
        }


        private static bool
            HasOnlyOneBestGroup(SortedDictionary<int, AdjacentSeatsGroups> bestGroups)
        {
            return bestGroups.FirstOrDefault().Value.Groups.Count == 1;
        }

        private static bool
            IsMatchingPartyRequested(SuggestionRequest suggestionRequest, ICollection seatWithDistances)
        {
            return seatWithDistances.Count >= suggestionRequest.PartyRequested;
        }

        private static AdjacentSeatsGroups
            SplitInGroupsOfAdjacentSeats(SuggestionRequest suggestionRequest,
                IEnumerable<SeatWithDistance> seatsWithDistances)
        {
            var adjacentSeats = new AdjacentSeats();
            var groupsOfAdjacentSeats = new AdjacentSeatsGroups();
            SeatWithDistance seatWithDistancePrevious = null;

            foreach (var seatWithDistance in seatsWithDistances.OrderBy(s => s.Seat.Number))
                if (seatWithDistancePrevious == null)
                {
                    seatWithDistancePrevious = seatWithDistance;
                    adjacentSeats.AddSeat(seatWithDistancePrevious);
                }
                else
                {
                    if (seatWithDistance.Seat.Number == seatWithDistancePrevious.Seat.Number + 1)
                    {
                        adjacentSeats.AddSeat(seatWithDistance);
                        seatWithDistancePrevious = seatWithDistance;
                    }
                    else
                    {
                        groupsOfAdjacentSeats.Groups.Add(adjacentSeats);
                        adjacentSeats = new AdjacentSeats();
                        adjacentSeats.AddSeat(seatWithDistance);
                        seatWithDistancePrevious = null;
                    }
                }

            groupsOfAdjacentSeats.Groups.Add(adjacentSeats);

            return SortGroupsByDistanceFromMiddleOfTheRow(suggestionRequest, groupsOfAdjacentSeats);
        }
    }
}