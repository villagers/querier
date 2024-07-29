using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier
{
    public interface IQuery<TContext> where TContext : DbContext
    {
        IQuery<TType, TContext> Create<TType>() where TType : class;


        List<QueryProperty> ListMeasures<TType>();
        List<QueryProperty> ListMeasures(Type type);
        List<QueryProperty> ListMeasures(string queryKey);


        List<QueryProperty> ListDimensions<TType>();
        List<QueryProperty> ListDimensions(Type type);
        List<QueryProperty> ListDimensions(string queryKey);


        List<QueryProperty> ListTimeDimensions<TType>();
        List<QueryProperty> ListTimeDimensions(Type type);
        List<QueryProperty> ListTimeDimensions(string queryKey);
    }
    public interface IQuery<TType, TContext> where TContext : DbContext where TType : class
    {
        IQuery<TType, TContext> Filter(string property, string op, object? args);
        IQuery<TType, TContext> Measure(string property, string? orderBy = null);
        IQuery<TType, TContext> Dimension(string property);
        IQuery<TType, TContext> TimeDimension(string property);
        IQuery<TType, TContext> TimeDimension(string property, TimeDimensionPart timeDimensionPart);
        IQuery<TType, TContext> OrderBy(string property, string direction);
        IQuery<TType, TContext> Limit(int limit);
        QueryResult Execute();

    }
}
