using System.Threading.Tasks;
using ExternalDependencies;
using Microsoft.AspNetCore.Mvc;

namespace SeatReservations.Api.Controllers
{
    [Route("api/data_for_reservation_seats/")]
    [ApiController]
    public class ReservationSeatsController : ControllerBase
    {
        private readonly IProvideCurrentReservations _provideCurrentReservations;

        public ReservationSeatsController(IProvideCurrentReservations provideCurrentReservations)
        {
            _provideCurrentReservations = provideCurrentReservations;
        }

        // GET api/data_for_reservation_seats/5
        [HttpGet("{showId}")]
        public async Task<ReservedSeatsDto> Get(string showId)
        {
            return await _provideCurrentReservations.GetReservedSeats(showId);
        }
    }
}