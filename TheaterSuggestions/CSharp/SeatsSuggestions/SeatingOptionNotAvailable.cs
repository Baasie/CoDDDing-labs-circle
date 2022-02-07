namespace SeatsSuggestions
{
    internal class SeatingOptionNotAvailable : SeatingOptionSuggested
    {
        public SeatingOptionNotAvailable(int partyRequested, PricingCategory pricingCategory) : base(partyRequested,
            pricingCategory)
        {
        }
    }
}