using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ExternalDependencies;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SeatsSuggestions.Infra.Adapter
{
    /// <summary>
    ///     Get via a web api and adapt reservations and return <see cref="ReservedSeatsDto" />.
    /// </summary>
    public class SeatReservationsWebRepository : IProvideCurrentReservations
    {
        private readonly string _uriSeatReservationService;

        public SeatReservationsWebRepository(string uriSeatReservationService)
        {
            _uriSeatReservationService = uriSeatReservationService;
        }

        public async Task<ReservedSeatsDto> GetReservedSeats(string showId)
        {
            var jsonSeatReservations = await GetDataForReservations(showId);

            var reservationsSeatsDto = JsonConvert
                .DeserializeObject<ReservedSeatsDto>(jsonSeatReservations,
                    new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            return reservationsSeatsDto;
        }

        private async Task<string> GetDataForReservations(string showId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_uriSeatReservationService);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync($"api/data_for_reservation_seats/{showId}");

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}