using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeatsSuggestions
{
    public class SeatAllocator
    {
        private const int NumberOfSuggestionsPerPricingCategory = 3;
        private readonly IProvideAuditoriumSeating _auditoriumSeatingAdapter;

        public SeatAllocator(IProvideAuditoriumSeating auditoriumSeatingAdapter)
        {
            _auditoriumSeatingAdapter = auditoriumSeatingAdapter;
        }

        public async Task<SuggestionsMade> MakeSuggestions(string showId, int partyRequested)
        {
            var auditoriumSeating = await _auditoriumSeatingAdapter.GetAuditoriumSeating(showId);

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

            for (var i = 0; i < NumberOfSuggestionsPerPricingCategory; i++)
            {
                var seatOptionsSuggested = auditoriumSeating
                    .SuggestSeatingOptionFor(new SuggestionRequest(partyRequested, pricingCategory));

                if (seatOptionsSuggested.MatchExpectation())
                {
                    // We get the new version of the Auditorium after the allocation
                    auditoriumSeating = auditoriumSeating.Allocate(seatOptionsSuggested);

                    foundedSuggestions.Add(new SuggestionMade(partyRequested, pricingCategory,
                        seatOptionsSuggested.Seats));
                }
            }

            return foundedSuggestions;
        }
    }
}