using NFluent;
using NUnit.Framework;
using SeatsSuggestions.Domain;

namespace SeatsSuggestions.Tests.UnitTests
{
    [TestFixture]
    public class SeatShould
    {
        [Test]
        public void Be_a_Value_Type()
        {
            var firstInstance = new Seat("A", 1, PricingCategory.Second, SeatAvailability.Available);
            var secondInstance = new Seat("A", 1, PricingCategory.Second, SeatAvailability.Available);

            // Two different instances with same values should be equals
            Check.That(secondInstance).IsEqualTo(firstInstance);

            // Should not mutate existing instance 
            secondInstance.Allocate();
            Check.That(secondInstance).IsEqualTo(firstInstance);
        }
    }
}