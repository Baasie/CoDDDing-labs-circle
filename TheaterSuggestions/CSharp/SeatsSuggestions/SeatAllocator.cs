using System.Collections.Generic;

namespace SeatsSuggestions
{
    public class SeatAllocator
    {
        private const int NumberOfSuggestions = 3;
        private readonly AuditoriumSeatingAdapter _auditoriumSeatingAdapter;

        public SeatAllocator(AuditoriumSeatingAdapter auditoriumSeatingAdapter)
        {
            _auditoriumSeatingAdapter = auditoriumSeatingAdapter;
        }

        public SuggestionsMade MakeSuggestions(string showId, int partyRequested)
        {
            var auditoriumSeating = _auditoriumSeatingAdapter.GetAuditoriumSeating(showId);

            var suggestionsMade = new SuggestionsMade(showId, partyRequested);

            suggestionsMade.Add(GiveMeSuggestionsFor(auditoriumSeating, partyRequested, PricingCategory.First));
            suggestionsMade.Add(GiveMeSuggestionsFor(auditoriumSeating, partyRequested, PricingCategory.Second));
            suggestionsMade.Add(GiveMeSuggestionsFor(auditoriumSeating, partyRequested, PricingCategory.Third));
            suggestionsMade.Add(GiveMeSuggestionsFor(auditoriumSeating, partyRequested, PricingCategory.Mixed));

            if (suggestionsMade.MatchExpectations())
            {
                return suggestionsMade;
            }

            return new SuggestionNotAvailable(showId, partyRequested);
        }

        private static IEnumerable<SuggestionMade> GiveMeSuggestionsFor(
            AuditoriumSeating auditoriumSeating,
            int partyRequested,
            PricingCategory pricingCategory)
        {
            var foundedSuggestions = new List<SuggestionMade>();

            for (var i = 0; i < NumberOfSuggestions; i++)
            {
                var seatAllocation = auditoriumSeating.SuggestSeatingOptionFor(partyRequested, pricingCategory);

                if (seatAllocation.MatchExpectation())
                {
                    foreach (var seat in seatAllocation.Seats)
                    {
                        seat.Allocate();
                    }

                    foundedSuggestions.Add(new SuggestionMade(partyRequested, pricingCategory, seatAllocation.Seats));
                }
            }

            return foundedSuggestions;
        }
    }
}