using System.Collections.Generic;

namespace ExternalDependencies.AuditoriumLayoutRepository
{
    public class AuditoriumDto
    {
        public Dictionary<string, IReadOnlyList<SeatDto>> Rows { get; set; }
        public IEnumerable<CorridorDto> Corridors { get; set; }
    }
}