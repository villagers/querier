using Org.BouncyCastle.Tls;
using Querier.Attributes;
using Querier.SqlQuery.Functions;
using Querier.SqlQuery.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Interfaces
{
    public interface IQuery : IQueryExecute
    {
        IQuery New();
        IQuery From(string table);

        IQuery MeasureCount(string property, string? propertyAs = null, string? orderBy = null);
        IQuery MeasureSum(string property, string? propertyAs = null, string? orderBy = null);
        IQuery MeasureAvg(string property, string? propertyAs = null, string? orderBy = null);
        IQuery MeasureMin(string property, string? propertyAs = null, string? orderBy = null);
        IQuery MeasureMax(string property, string? propertyAs = null, string? orderBy = null);

        IQuery Dimension(string property, string? propertyAs = null);

        IQuery TimeDimension(string property, string? propertyAs = null);
        IQuery TimeDimension(string property, string timeDimensionPart, string? propertyAs = null);

        IQuery OrderBy(string property, string direction);
        IQuery Limit(int limit);

        IQuery Filter(Func<IQueryFilter, IQueryFilter> filter);

        List<Dictionary<string, string>> GetMeasures<TType>();
        List<Dictionary<string, string>> GetMeasures(string queryKey);
        List<Dictionary<string, string>> GetDimensions<TType>();
        List<Dictionary<string, string>> GetDimensions(string queryKey);
        List<Dictionary<string, string>> GetTimeDimensions<TType>();
        List<Dictionary<string, string>> GetTimeDimensions(string queryKey);
    }
}
