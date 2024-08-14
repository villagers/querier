using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier
{
    public interface IQuery
    {
        IQuery Create(string table);
        IQuery Filter(string property, string op, object? args);
        IQuery AndFilter(string property, string op, object? args);
        IQuery OrFilter(string property, string op, object? args);

        IQuery Measure(string aggregation, string property, string? orderBy = null);
        IQuery MeasureCount(string property, string? orderBy = null);
        IQuery MeasureSum(string property, string? orderBy = null);
        IQuery MeasureAvg(string property, string? orderBy = null);
        IQuery MeasureMin(string property, string? orderBy = null);
        IQuery MeasureMax(string property, string? orderBy = null);

        IQuery Dimension(string property);

        IQuery TimeDimension(string property);
        IQuery TimeDimension(string property, TimeDimensionPart timeDimensionPart);

        IQuery OrderBy(string property, string direction);
        IQuery Limit(int limit);

        List<QueryProperty> ListMeasures<TType>();
        List<QueryProperty> ListMeasures(Type type);
        List<QueryProperty> ListMeasures(string queryKey);


        List<QueryProperty> ListDimensions<TType>();
        List<QueryProperty> ListDimensions(Type type);
        List<QueryProperty> ListDimensions(string queryKey);


        List<QueryProperty> ListTimeDimensions<TType>();
        List<QueryProperty> ListTimeDimensions(Type type);
        List<QueryProperty> ListTimeDimensions(string queryKey);

        QueryResult Execute();

    }
}
