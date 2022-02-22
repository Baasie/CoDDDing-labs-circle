using System.Threading.Tasks;

namespace SeatsSuggestions.Domain.Port
{
    public interface IAdaptAuditoriumSeating
    {
        Task<AuditoriumSeating> GetAuditoriumSeating(ShowId showId);
    }
}