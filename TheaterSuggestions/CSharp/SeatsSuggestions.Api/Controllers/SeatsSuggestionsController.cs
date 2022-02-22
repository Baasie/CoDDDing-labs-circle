using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SeatsSuggestions.Domain;
using SeatsSuggestions.Domain.Port;

namespace SeatsSuggestions.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatsSuggestionsController : ControllerBase
    {
        private readonly IProvideSeatSuggestionsForShows _seatSuggestionsForShows;

        public SeatsSuggestionsController(IProvideSeatSuggestionsForShows seatSuggestionsForShows)
        {
            _seatSuggestionsForShows = seatSuggestionsForShows;
        }

        // GET api/SeatsSuggestions?showId=5&party=3
        [HttpGet]
        public async Task<ActionResult<string>> Get([FromQuery(Name = "showId")] string showId,
            [FromQuery(Name = "party")] int party)
        {
            // Infra => Domain
            var id = new ShowId(showId);
            var partyRequested = new PartyRequested(party);

            var suggestions = await _seatSuggestionsForShows.MakeSuggestions(id, partyRequested);

            // Domain => Infra
            return JsonConvert.SerializeObject(suggestions, Formatting.Indented);
        }
    }
}