using System.Collections.Generic;
using ExternalDependencies;

namespace SeatsSuggestions
{
    /// <summary>
    ///     Adapt Dtos coming from the external dependencies (ReservationsProvider, AuditoriumLayoutRepository) to
    ///     AuditoriumSeating instances.
    /// </summary>
    public class AuditoriumSeatingAdapter
    {
        private readonly IProvideCurrentReservations _reservedSeatsRepository;
        private readonly IProvideAuditoriumLayouts _auditoriumLayoutRepository;

        public AuditoriumSeatingAdapter(IProvideAuditoriumLayouts auditoriumLayoutRepository,
            IProvideCurrentReservations reservationsProvider)
        {
            _auditoriumLayoutRepository = auditoriumLayoutRepository;
            _reservedSeatsRepository = reservationsProvider;
        }

        public AuditoriumSeating GetAuditoriumSeating(string showId)
        {
            return Adapt(_auditoriumLayoutRepository.GetAuditoriumSeatingFor(showId),
                _reservedSeatsRepository.GetReservedSeats(showId));
        }

        private static AuditoriumSeating Adapt(AuditoriumDto auditoriumDto, ReservedSeatsDto reservedSeatsDto)
        {
            var rows = new Dictionary<string, Row>();

            foreach (var rowDto in auditoriumDto.Rows)
            {
                var seats = new List<Seat>();

                foreach (var seatDto in rowDto.Value)
                {
                    var rowName = rowDto.Key;
                    var number = ExtractNumber(seatDto.Name);
                    var pricingCategory = ConvertCategory(seatDto.Category);

                    var isReserved = reservedSeatsDto.ReservedSeats.Contains(seatDto.Name);

                    seats.Add(new Seat(rowName, number, pricingCategory,
                        isReserved ? SeatAvailability.Reserved : SeatAvailability.Available));
                }

                rows[rowDto.Key] = new Row(rowDto.Key, seats);
            }

            return new AuditoriumSeating(rows);
        }

        private static PricingCategory ConvertCategory(int seatDtoCategory)
        {
            return (PricingCategory) seatDtoCategory;
        }

        private static uint ExtractNumber(string name)
        {
            return uint.Parse(name.Substring(1));
        }
    }
}