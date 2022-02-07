namespace SeatsSuggestions.Tests
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

        public bool IsAvailable => SeatAvailability == SeatAvailability.Available;

        public override string ToString()
        {
            return $"{RowName}{Number}";
        }

        public void UpdateCategory(SeatAvailability seatAvailability)
        {
            SeatAvailability = seatAvailability;
        }
    }
}