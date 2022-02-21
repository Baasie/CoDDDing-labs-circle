using ExternalDependencies.AuditoriumLayoutRepository;
using ExternalDependencies.ReservationsProvider;
using NFluent;
using NUnit.Framework;

namespace SeatsSuggestions.Tests.AcceptanceTests
{
    [TestFixture]
    public class SeatAllocatorShould
    {
        [Test]
        public void Return_SeatsNotAvailable_when_Auditorium_has_all_its_seats_already_reserved()
        {
            // Madison Auditorium-5
            //      1   2   3   4   5   6   7   8   9  10
            // A : (2) (2) (1) (1) (1) (1) (1) (1) (2) (2)
            // B : (2) (2) (1) (1) (1) (1) (1) (1) (2) (2)
            const string showId = "5";
            const int partyRequested = 1;

            var auditoriumLayoutAdapter =
                new AuditoriumSeatingAdapter(new AuditoriumLayoutRepository(), new ReservationsProvider());

            var seatAllocator = new SeatAllocator(auditoriumLayoutAdapter);

            var suggestionsMade = seatAllocator.MakeSuggestions(showId, partyRequested);
            Check.That(suggestionsMade.PartyRequested).IsEqualTo(partyRequested);
            Check.That(suggestionsMade.ShowId).IsEqualTo(showId);

            Check.That(suggestionsMade).IsInstanceOf<SuggestionNotAvailable>();
        }

        [Test]
        public void Suggest_one_seat_when_Auditorium_contains_one_available_seat_only()
        {
            // Ford Auditorium-1
            //
            //       1   2   3   4   5   6   7   8   9  10
            //  A : (2) (2)  1  (1) (1) (1) (1) (1) (2) (2)
            //  B : (2) (2) (1) (1) (1) (1) (1) (1) (2) (2)
            const string showId = "1";
            const int partyRequested = 1;

            var auditoriumLayoutAdapter =
                new AuditoriumSeatingAdapter(new AuditoriumLayoutRepository(), new ReservationsProvider());

            var seatAllocator = new SeatAllocator(auditoriumLayoutAdapter);

            var suggestionsMade = seatAllocator.MakeSuggestions(showId, partyRequested);

            Check.That(suggestionsMade.SeatNames(PricingCategory.First)).ContainsExactly("A3");
        }

        [Test]
        public void Offer_several_suggestions_ie_1_per_PricingCategory_and_other_one_without_category_affinity()
        {
            // New Amsterdam-18
            //
            //     1   2   3   4   5   6   7   8   9  10
            //  A: 2   2   1   1   1   1   1   1   2   2
            //  B: 2   2   1   1   1   1   1   1   2   2
            //  C: 2   2   2   2   2   2   2   2   2   2
            //  D: 2   2   2   2   2   2   2   2   2   2
            //  E: 3   3   3   3   3   3   3   3   3   3
            //  F: 3   3   3   3   3   3   3   3   3   3
            const string showId = "18";
            const int partyRequested = 1;

            var auditoriumLayoutAdapter =
                new AuditoriumSeatingAdapter(new AuditoriumLayoutRepository(), new ReservationsProvider());

            var seatAllocator = new SeatAllocator(auditoriumLayoutAdapter);

            var suggestionsMade = seatAllocator.MakeSuggestions(showId, partyRequested);

            Check.That(suggestionsMade.SeatNames(PricingCategory.First)).ContainsExactly("A5", "A6", "A4");
            Check
                .That(suggestionsMade.SeatNames(PricingCategory.Second)).ContainsExactly("A2", "A9", "A1");
            Check.That(suggestionsMade.SeatNames(PricingCategory.Third)).ContainsExactly("E5", "E6", "E4");

            Check
                .That(suggestionsMade.SeatNames(PricingCategory.Mixed)).ContainsExactly("A5", "A6", "A4");
        }

        [Test]
        public void Offer_seats_nearer_the_middle_of_a_row()
        {
            // Mogador Auditorium-9
            //
            //    1   2   3   4   5   6   7   8   9  10
            // A: 2   2   1   1  (1) (1) (1) (1)  2   2
            // B: 2   2   1   1   1   1   1   1   2   2
            const string showId = "9";
            const int partyRequested = 1;

            var auditoriumLayoutAdapter =
                new AuditoriumSeatingAdapter(new AuditoriumLayoutRepository(), new ReservationsProvider());

            var seatAllocator = new SeatAllocator(auditoriumLayoutAdapter);

            var suggestionsMade = seatAllocator.MakeSuggestions(showId, partyRequested);

            Check.That(suggestionsMade.SeatNames(PricingCategory.First))
                .ContainsExactly("A4", "A3", "B5");
        }

        [Test]
        public void Offer_4_adjacent_seats_nearer_the_middle_of_a_row_when_it_is_possible()
        {
            // Dock Street Auditorium-3
            //
            //      1   2   3   4   5   6   7   8   9  10
            // A:  (2) (2) (1) (1) (1)  1   1   1   2   2
            // B:   2   2   1   1  (1) (1) (1) (1)  2   2
            // C:   2   2   2   2   2   2   2   2   2   2
            // D:   2   2   2   2   2   2   2   2   2   2
            // E:   3   3   3   3   3   3   3   3   3   3
            // F:   3   3   3   3   3   3   3   3   3   3
            var showId = "3";
            var partyRequested = 4;

            var auditoriumLayoutAdapter =
                new AuditoriumSeatingAdapter(new AuditoriumLayoutRepository(), new ReservationsProvider());

            var seatAllocator = new SeatAllocator(auditoriumLayoutAdapter);

            var suggestionsMade = seatAllocator.MakeSuggestions(showId, partyRequested);

            Check.That(suggestionsMade.SeatNames(PricingCategory.First)).IsEmpty();
            Check.That(suggestionsMade.SeatNames(PricingCategory.Second)).ContainsExactly("C4-C5-C6-C7", "D4-D5-D6-D7");
            Check.That(suggestionsMade.SeatNames(PricingCategory.Third)).ContainsExactly("E4-E5-E6-E7", "F4-F5-F6-F7");
            Check.That(suggestionsMade.SeatNames(PricingCategory.Mixed))
                .ContainsExactly("A6-A7-A8-A9", "B1-B2-B3-B4", "C4-C5-C6-C7");
        }

        [Test]
        public void Offer_3_adjacent_seats_nearer_the_middle_of_a_row_when_it_is_possible()
        {
            // Dock Street Auditorium-3
            //
            //      1   2   3   4   5   6   7   8   9  10
            // A:  (2) (2) (1) (1) (1)  1   1   1   2   2
            // B:   2   2   1   1  (1) (1) (1) (1)  2   2
            // C:   2   2   2   2   2   2   2   2   2   2
            // D:   2   2   2   2   2   2   2   2   2   2
            // E:   3   3   3   3   3   3   3   3   3   3
            // F:   3   3   3   3   3   3   3   3   3   3
            var showId = "3";
            var partyRequested = 3;

            var auditoriumLayoutAdapter =
                new AuditoriumSeatingAdapter(new AuditoriumLayoutRepository(), new ReservationsProvider());

            var seatAllocator = new SeatAllocator(auditoriumLayoutAdapter);

            var suggestionsMade = seatAllocator.MakeSuggestions(showId, partyRequested);

            Check.That(suggestionsMade.SeatNames(PricingCategory.First)).ContainsExactly("A6-A7-A8");
            Check.That(suggestionsMade.SeatNames(PricingCategory.Second))
                .ContainsExactly("C4-C5-C6", "C7-C8-C9", "C1-C2-C3");
            Check.That(suggestionsMade.SeatNames(PricingCategory.Third))
                .ContainsExactly("E4-E5-E6", "E7-E8-E9", "E1-E2-E3");
            Check.That(suggestionsMade.SeatNames(PricingCategory.Mixed))
                .ContainsExactly("A6-A7-A8", "B2-B3-B4", "C4-C5-C6");
        }

        [Test]
        public void Offer_2_adjacent_seats_nearer_the_middle_of_a_row_when_it_is_possible()
        {
            // Dock Street Auditorium-3
            //
            //      1   2   3   4   5   6   7   8   9  10
            // A:  (2) (2) (1) (1) (1)  1   1   1   2   2
            // B:   2   2   1   1  (1) (1) (1) (1)  2   2
            // C:   2   2   2   2   2   2   2   2   2   2
            // D:   2   2   2   2   2   2   2   2   2   2
            // E:   3   3   3   3   3   3   3   3   3   3
            // F:   3   3   3   3   3   3   3   3   3   3
            var showId = "3";
            var partyRequested = 2;

            var auditoriumLayoutAdapter =
                new AuditoriumSeatingAdapter(new AuditoriumLayoutRepository(), new ReservationsProvider());

            var seatAllocator = new SeatAllocator(auditoriumLayoutAdapter);

            var suggestionsMade = seatAllocator.MakeSuggestions(showId, partyRequested);

            Check.That(suggestionsMade.SeatNames(PricingCategory.First))
                .ContainsExactly("A6-A7", "B3-B4");
            Check.That(suggestionsMade.SeatNames(PricingCategory.Second))
                .ContainsExactly("A9-A10", "B1-B2", "B9-B10");
            Check.That(suggestionsMade.SeatNames(PricingCategory.Third))
                .ContainsExactly("E5-E6", "E3-E4", "E7-E8");
            Check.That(suggestionsMade.SeatNames(PricingCategory.Mixed))
                .ContainsExactly("A6-A7", "A8-A9", "B3-B4");
        }
    }
}