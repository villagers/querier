using Querier.Options;
using Querier.Schema;

namespace Querier.Interfaces
{
    public interface IQuery : IQueryExecute
    {
        IQuery New();
        IQuery From(string table);

        IQuery Measure(string property, string? propertyAs = null);
        IQuery MeasureCount(string property, string? propertyAs = null);
        IQuery MeasureSum(string property, string? propertyAs = null);
        IQuery MeasureAvg(string property, string? propertyAs = null);
        IQuery MeasureMin(string property, string? propertyAs = null);
        IQuery MeasureMax(string property, string? propertyAs = null);

        IQuery Dimension(string property, string? propertyAs = null);

        IQuery TimeDimension(string property, string? propertyAs = null);
        IQuery TimeDimension(string property, string timeDimensionPart, string? propertyAs = null);

        IQuery OrderBy(string property, string direction);
        IQuery Limit(int limit);

        IQuery FilterRaw(string sql);
        IQuery Filter(string column, Func<IQueryFilter, IQueryFilter> filter);

        IQuery Union(Func<IQuery, IQuery> query);

        IQuery FillMissingDates(DateTime fromDate, DateTime toDate, Dictionary<string, List<object>> columnValues, FillMissingOption? options = null);

        HashSet<QueryMeasureSchema> GetMeasures<TType>();
        HashSet<QueryMeasureSchema> GetMeasures(string queryKey);
        HashSet<QueryDimensionSchema> GetDimensions<TType>();
        HashSet<QueryDimensionSchema> GetDimensions(string queryKey);
        HashSet<QueryTimeDimensionSchema> GetTimeDimensions<TType>();
        HashSet<QueryTimeDimensionSchema> GetTimeDimensions(string queryKey);
    }
}
