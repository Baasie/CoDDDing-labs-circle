namespace ExternalDependencies
{
    public interface IProvideAuditoriumLayouts
    {
        AuditoriumDto GetAuditoriumSeatingFor(string showId);
    }
}