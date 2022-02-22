using NFluent;
using NUnit.Framework;
using SeatsSuggestions.Domain;
using SeatsSuggestions.Domain.DeepModel;

namespace SeatsSuggestions.Tests.UnitTests.DeepModel
{
    internal class SeatWithDistanceFromTheMiddleOfTheRowShould
    {
        [Test]
        public void Be_a_Value_Type()
        {
            var firstInstance = new SeatWithDistance(
                new Seat("A", 1, PricingCategory.Second, SeatAvailability.Available),
                5);
            var secondInstance = new SeatWithDistance(
                new Seat("A", 1, PricingCategory.Second, SeatAvailability.Available),
                5);

            Check.That(firstInstance).IsEqualTo(secondInstance);
            Check.That(firstInstance.ToString()).IsEqualTo(secondInstance.ToString());
        }
    }
}