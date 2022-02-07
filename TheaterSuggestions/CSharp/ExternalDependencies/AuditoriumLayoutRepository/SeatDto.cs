namespace ExternalDependencies.AuditoriumLayoutRepository
{
    public class SeatDto
    {
        public string Name { get; }
        public int Category { get; }

        public SeatDto(string name, int category)
        {
            Name = name;
            Category = category;
        }
    }
}