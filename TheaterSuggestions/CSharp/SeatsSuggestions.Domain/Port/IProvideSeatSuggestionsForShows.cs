using System.Threading.Tasks;

namespace SeatsSuggestions.Domain.Port
{
    public interface IProvideSeatSuggestionsForShows
    {
        Task<SuggestionsMade> MakeSuggestions(ShowId showId, PartyRequested partyRequested);
    }
}