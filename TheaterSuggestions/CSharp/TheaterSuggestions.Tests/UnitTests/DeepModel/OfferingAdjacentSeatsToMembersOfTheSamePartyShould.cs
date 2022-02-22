using System.Collections.Generic;
using NFluent;
using NUnit.Framework;
using SeatsSuggestions.Domain;

namespace SeatsSuggestions.Tests.UnitTests.DeepModel
{
    internal class OfferingAdjacentSeatsToMembersOfTheSamePartyShould
    {
        [Test]
        public void Offer_3_adjacent_seats_nearer_the_middle_of_the_row_when_the_middle_is_not_reserved()
        {
            var a1 = new Seat("A", 1, PricingCategory.Second, SeatAvailability.Available);
            var a2 = new Seat("A", 2, PricingCategory.Second, SeatAvailability.Available);
            var a3 = new Seat("A", 3, PricingCategory.First, SeatAvailability.Available);
            var a4 = new Seat("A", 4, PricingCategory.First, SeatAvailability.Reserved);
            var a5 = new Seat("A", 5, PricingCategory.First, SeatAvailability.Available);
            var a6 = new Seat("A", 6, PricingCategory.First, SeatAvailability.Available);
            var a7 = new Seat("A", 7, PricingCategory.First, SeatAvailability.Available);
            var a8 = new Seat("A", 8, PricingCategory.First, SeatAvailability.Reserved);
            var a9 = new Seat("A", 9, PricingCategory.Second, SeatAvailability.Available);
            var a10 = new Seat("A", 10, PricingCategory.Second, SeatAvailability.Available);

            var row = new Row("A", new List<Seat> { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10 });

            Check.That(row.OfferAdjacentSeatsNearerTheMiddleOfTheRow(new SuggestionRequest(new PartyRequested(3),
                    PricingCategory.Mixed)))
                .ContainsExactly(a5, a6, a7);
        }

        [Test]
        public void Offer_2_adjacent_seats_nearer_the_middle_of_the_row_when_the_middle_is_already_reserved()
        {
            var a1 = new Seat("A", 1, PricingCategory.Second, SeatAvailability.Available);
            var a2 = new Seat("A", 2, PricingCategory.Second, SeatAvailability.Available);
            var a3 = new Seat("A", 3, PricingCategory.First, SeatAvailability.Reserved);
            var a4 = new Seat("A", 4, PricingCategory.First, SeatAvailability.Reserved);
            var a5 = new Seat("A", 5, PricingCategory.First, SeatAvailability.Reserved);
            var a6 = new Seat("A", 6, PricingCategory.First, SeatAvailability.Reserved);
            var a7 = new Seat("A", 7, PricingCategory.First, SeatAvailability.Reserved);
            var a8 = new Seat("A", 8, PricingCategory.First, SeatAvailability.Reserved);
            var a9 = new Seat("A", 9, PricingCategory.Second, SeatAvailability.Available);
            var a10 = new Seat("A", 10, PricingCategory.Second, SeatAvailability.Available);

            var row = new Row("A", new List<Seat> { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10 });

            Check.That(row.OfferAdjacentSeatsNearerTheMiddleOfTheRow(new SuggestionRequest(new PartyRequested(2)
                    , PricingCategory.Mixed)))
                .ContainsExactly(a2, a1);
        }
    }
}