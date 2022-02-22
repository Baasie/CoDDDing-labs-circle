using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ExternalDependencies;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SeatsSuggestions
{
    /// <summary>
    ///     Get via a web api and adapt auditorium and return <see cref="AuditoriumDto" />.
    /// </summary>
    public class AuditoriumWebRepository : IProvideAuditoriumLayouts
    {
        private readonly string _uriAuditoriumSeatingRepository;

        public AuditoriumWebRepository(string uriAuditoriumSeatingRepository)
        {
            _uriAuditoriumSeatingRepository = uriAuditoriumSeatingRepository;
        }

        public async Task<AuditoriumDto> GetAuditoriumSeatingFor(string showId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_uriAuditoriumSeatingRepository);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync($"api/data_for_auditoriumSeating/{showId}");

                response.EnsureSuccessStatusCode();

                var jsonAuditoriumSeating = await response.Content.ReadAsStringAsync();

                var auditoriumSeatingDto = JsonConvert
                    .DeserializeObject<AuditoriumDto>(jsonAuditoriumSeating,
                        new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()});

                return auditoriumSeatingDto;
            }
        }
    }
}