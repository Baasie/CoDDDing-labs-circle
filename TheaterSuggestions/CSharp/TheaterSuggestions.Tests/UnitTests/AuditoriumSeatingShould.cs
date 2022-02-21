using System.Linq;
using ExternalDependencies.AuditoriumLayoutRepository;
using ExternalDependencies.ReservationsProvider;
using NFluent;
using NUnit.Framework;

namespace SeatsSuggestions.Tests.UnitTests
{
    [TestFixture]
    public class AuditoriumSeatingShould
    {
        [Test]
        public void Be_a_Value_Type()
        {
            var auditoriumLayoutAdapter =
                new AuditoriumSeatingAdapter(new AuditoriumLayoutRepository(), new ReservationsProvider());
            var showIdWithoutReservationYet = "18";
            var auditoriumSeatingFirstInstance =
                auditoriumLayoutAdapter.GetAuditoriumSeating(showIdWithoutReservationYet);
            var auditoriumSeatingSecondInstance =
                auditoriumLayoutAdapter.GetAuditoriumSeating(showIdWithoutReservationYet);

            // Two different instances with same values should be equals
            Check.That(auditoriumSeatingSecondInstance).IsEqualTo(auditoriumSeatingFirstInstance);

            // Should not mutate existing instance 
            auditoriumSeatingSecondInstance.Rows.Values.First().Seats.First().Allocate();
            Check.That(auditoriumSeatingSecondInstance).IsEqualTo(auditoriumSeatingFirstInstance);
        }
    }
}