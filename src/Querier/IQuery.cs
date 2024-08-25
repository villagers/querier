using Org.BouncyCastle.Tls;
using Querier.SqlQuery.Functions;
using Querier.SqlQuery.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier
{
    public interface IQuery
    {
        IQuery New();
        IQuery From(string table);

        IQuery MeasureCount(string property, string? propertyAs = null, string? orderBy = null);
        IQuery MeasureSum(string property, string? propertyAs = null, string? orderBy = null);
        IQuery MeasureAvg(string property, string? propertyAs = null, string? orderBy = null);
        IQuery MeasureMin(string property, string? propertyAs = null, string? orderBy = null);
        IQuery MeasureMax(string property, string? propertyAs = null, string? orderBy = null);

        IQuery Dimension(string property);

        IQuery TimeDimension(string property);
        IQuery TimeDimension(string property, string timeDimensionPart);

        IQuery OrderBy(string property, string direction);
        IQuery Limit(int limit);

        IQuery Filter(Func<IQueryFilter, IQueryFilter> filter);

        List<QueryProperty> GetMeasures<TType>();
        List<QueryProperty> GetDimensions<TType>();
        List<QueryProperty> GetTimeDimensions<TType>();

        QueryResult Execute();

    }
}
