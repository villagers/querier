namespace Querier
{
    public class QueryMeasure
    {
        public required string Property { get; set; }
        public string? PropertyAs { get; set; }
        public string? OrderBy { get; set; }

        public string SqlColumn => PropertyAs ?? Property;
    }
}
