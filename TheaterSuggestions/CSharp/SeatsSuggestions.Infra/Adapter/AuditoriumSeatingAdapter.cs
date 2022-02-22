using System.Collections.Generic;
using System.Threading.Tasks;
using ExternalDependencies;
using SeatsSuggestions.Domain;
using SeatsSuggestions.Domain.Port;

namespace SeatsSuggestions.Infra.Adapter
{
    /// <summary>
    ///     Adapt Dtos coming from the external dependencies (ReservationsProvider, AuditoriumLayoutRepository) to
    ///     AuditoriumSeating instances.
    /// </summary>
    public class AuditoriumSeatingAdapter : IAdaptAuditoriumSeating
    {
        private readonly IProvideAuditoriumLayouts _auditoriumSeatingRepository;
        private readonly IProvideCurrentReservations _reservationsProvider;

        public AuditoriumSeatingAdapter(IProvideAuditoriumLayouts auditoriumSeatingRepository,
            IProvideCurrentReservations reservationsProvider)
        {
            _auditoriumSeatingRepository = auditoriumSeatingRepository;
            _reservationsProvider = reservationsProvider;
        }

        public async Task<AuditoriumSeating> GetAuditoriumSeating(ShowId showId)
        {
            var auditoriumDto = await _auditoriumSeatingRepository.GetAuditoriumSeatingFor(showId.Id);
            var reservedSeatsDto = await _reservationsProvider.GetReservedSeats(showId.Id);
            return AdaptAuditoriumSeatingDto(auditoriumDto, reservedSeatsDto);
        }


        private static AuditoriumSeating AdaptAuditoriumSeatingDto(AuditoriumDto auditoriumDto,
            ReservedSeatsDto reservedSeatsDto)
        {
            var rows = new Dictionary<string, Row>();

            foreach (var (rowName, seatDtos) in auditoriumDto.Rows)
            {
                var seats = new List<Seat>();
                foreach (var seatDto in seatDtos)
                {
                    var number = ExtractSeatNumber(seatDto.Name);
                    var priceCategory = ConvertCategory(seatDto.Category);

                    var isReservationsSeat = reservedSeatsDto.ReservedSeats.Contains(seatDto.Name);

                    seats.Add(new Seat(rowName, number, priceCategory,
                        isReservationsSeat ? SeatAvailability.Reserved : SeatAvailability.Available));
                }

                rows[rowName] = new Row(rowName, seats);
            }

            return new AuditoriumSeating(rows);
        }

        private static PricingCategory ConvertCategory(int dtoPricingCategory)
        {
            return (PricingCategory)dtoPricingCategory;
        }

        private static uint ExtractSeatNumber(string seatName)
        {
            return uint.Parse(seatName.Substring(1));
        }
    }
}