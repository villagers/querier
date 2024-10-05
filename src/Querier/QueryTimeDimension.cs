namespace Querier
{
    public class QueryTimeDimension
    {
        public required string Property { get; set; }
        public string? PropertyAs { get; set; }
        public string? OrderBy { get; set; }
        public string? TimeDimensionPart { get; set; }

        public string SqlColumn => PropertyAs ?? Property;
    }
}
