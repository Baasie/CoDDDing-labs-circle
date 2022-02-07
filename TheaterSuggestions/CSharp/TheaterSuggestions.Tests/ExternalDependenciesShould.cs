using ExternalDependencies.AuditoriumLayoutRepository;
using ExternalDependencies.ReservationsProvider;
using NFluent;
using NUnit.Framework;

namespace SeatsSuggestions.Tests
{
    [TestFixture]
    public class ExternalDependenciesShould
    {
        [Test]
        public void Allow_us_to_retrieve_reserved_seats_for_a_given_ShowId()
        {
            var seatsRepository = new ReservationsProvider();
            var reservedSeatsDto = seatsRepository.GetReservedSeats(showId: "1");

            Check.That(reservedSeatsDto.ReservedSeats).HasSize(19);
        }

        [Test]
        public void Allow_us_to_retrieve_AuditoriumLayout_for_a_given_ShowId()
        {
            var eventRepository = new AuditoriumLayoutRepository();
            var theaterDto = eventRepository.GetAuditoriumLayoutFor(showId: "2");

            Check.That(theaterDto.Rows).HasSize(6);
            Check.That(theaterDto.Corridors).HasSize(2);
            var firstSeatOfFirstRow = theaterDto.Rows["A"][0];
            Check.That(firstSeatOfFirstRow.Category).IsEqualTo(2);
        }
    }
}