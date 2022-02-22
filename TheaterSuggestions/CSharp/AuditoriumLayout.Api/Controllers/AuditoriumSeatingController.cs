using System.Threading.Tasks;
using ExternalDependencies;
using Microsoft.AspNetCore.Mvc;

namespace AuditoriumLayout.Api.Controllers
{
    [Route("api/data_for_auditoriumSeating/")]
    [ApiController]
    public class AuditoriumSeatingController : ControllerBase
    {
        private readonly IProvideAuditoriumLayouts _provideAuditoriumLayouts;

        public AuditoriumSeatingController(IProvideAuditoriumLayouts provideAuditoriumLayouts)
        {
            _provideAuditoriumLayouts = provideAuditoriumLayouts;
        }

        // GET api/data_for_auditoriumSeating/5
        [HttpGet("{showId}")]
        public async Task<ActionResult<AuditoriumDto>> Get(string showId)
        {
            return await _provideAuditoriumLayouts.GetAuditoriumSeatingFor(showId);
        }
    }
}