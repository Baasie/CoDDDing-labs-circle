namespace ExternalDependencies
{
    public interface IProvideCurrentReservations
    {
        ReservedSeatsDto GetReservedSeats(string showId);
    }
}