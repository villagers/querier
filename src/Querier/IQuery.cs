using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Tls;
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
