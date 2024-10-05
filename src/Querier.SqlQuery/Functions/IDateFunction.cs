namespace Querier.SqlQuery.Functions
{
    public interface IDateFunction
    {
        IFunction Date(string column, string? columnAs = null);
        IFunction Year(string column, string? columnAs = null);
        IFunction Month(string column, string? columnAs = null);
        IFunction Day(string column, string? columnAs = null);
        IFunction Hour(string column, string? columnAs = null);
        IFunction Minute(string column, string? columnAs = null);
        IFunction Second(string column, string? columnAs = null);
    }
}