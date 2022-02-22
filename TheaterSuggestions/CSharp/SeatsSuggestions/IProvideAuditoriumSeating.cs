using System.Threading.Tasks;

namespace SeatsSuggestions
{
    public interface IProvideAuditoriumSeating
    {
        Task<AuditoriumSeating> GetAuditoriumSeating(string showId);
    }
}