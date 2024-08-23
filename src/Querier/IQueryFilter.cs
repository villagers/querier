using Querier.SqlQuery.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier
{
    public interface IQueryFilter
    {
        IQueryFilter New();
        IQueryFilter In<T>(string column, IEnumerable<T> value);
        IQueryFilter Equal<T>(string column, T value);
        IQueryFilter Equal(Func<IFunction, IFunction> function);
        IQueryFilter Contains<T>(string column, T value);
        IQueryFilter StartsWith<T>(string column, T value);
        IQueryFilter EndsWith<T>(string column, T value);
        IQueryFilter Greater<T>(string column, T value);
        IQueryFilter GreaterOrEqual<T>(string column, T value);
        IQueryFilter Less<T>(string column, T value);
        IQueryFilter LessOrEqual<T>(string column, T value);

        IQueryFilter And();
        IQueryFilter And<T>(T value);
        IQueryFilter Or();
        IQueryFilter Or<T>(T value);
    }
}
