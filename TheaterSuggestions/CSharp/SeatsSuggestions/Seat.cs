namespace SeatsSuggestions
{
    public class Seat
    {
        public Seat(string rowName, uint number, PricingCategory pricingCategory, SeatAvailability seatAvailability)
        {
            RowName = rowName;
            Number = number;
            PricingCategory = pricingCategory;
            SeatAvailability = seatAvailability;
        }

        public string RowName { get; }
        public uint Number { get; }
        public PricingCategory PricingCategory { get; }
        private SeatAvailability SeatAvailability { get; set; }

        public bool IsAvailable()
        {
            return SeatAvailability == SeatAvailability.Available;
        }

        public override string ToString()
        {
            return $"{RowName}{Number}";
        }

        public bool MatchCategory(PricingCategory pricingCategory)
        {
            if (pricingCategory == PricingCategory.Mixed)
            {
                return true;
            }

            return PricingCategory == pricingCategory;
        }

        public void Allocate()
        {
            if (SeatAvailability == SeatAvailability.Available)
            {
                SeatAvailability = SeatAvailability.Allocated;
            }
        }
    }
}