using System.Collections.Generic;
using Value;

namespace SeatsSuggestions
{
    public class Seat : ValueType<Seat>
    {
        public Seat(string rowName, uint number, PricingCategory pricingCategory, SeatAvailability seatAvailability)
        {
            RowName = rowName;
            Number = number;
            PricingCategory = pricingCategory;
            SeatAvailability = seatAvailability;
        }

        public string RowName { get; init; }
        public uint Number { get; init; }
        public PricingCategory PricingCategory { get; init; }
        public SeatAvailability SeatAvailability { get; init; }

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

        public Seat Allocate()
        {
            if (SeatAvailability == SeatAvailability.Available)
            {
                return new Seat(RowName, Number, PricingCategory, SeatAvailability.Allocated);
            }

            return this;
        }

        public bool SameSeatLocation(Seat seat)
        {
            return RowName == seat.RowName && Number == seat.Number;
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] {RowName, Number, PricingCategory, SeatAvailability};
        }
    }
}