using Querier.SqlQuery.Functions;
using Querier.SqlQuery.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier
{
    public class QueryFilter<TQuery> : IQueryFilter where TQuery : IBaseQuery<TQuery>
    {

        private readonly TQuery _query;

        public QueryFilter(TQuery query)
        {
            _query = query;
        }

        public IQueryFilter New()
        {
            return new QueryFilter<TQuery>(_query);
        }

        public IQueryFilter In<T>(string column, IEnumerable<T> value)
        {
            _query.WhereIn(column, value);
            return this;
        }
        public IQueryFilter In<T>(Func<IFunction, IFunction> function, IEnumerable<T> value)
        {
            _query.WhereIn(function, value);
            return this;
        }

        public IQueryFilter Equal<T>(string column, T value)
        {
            _query.WhereEqual(column, value);
            return this;
        }
        public IQueryFilter Equal<T>(Func<IFunction, IFunction> function, T value)
        {
            _query.WhereEqual(function, value);
            return this;
        }

        public IQueryFilter Contains<T>(string column, T value)
        {
            _query.WhereLike(column, value);
            return this;
        }
        public IQueryFilter Contains<T>(Func<IFunction, IFunction> function, T value)
        {
            _query.WhereLike(function, value);
            return this;
        }

        public IQueryFilter StartsWith<T>(string column, T value)
        {
            _query.WhereStarts(column, value);
            return this;
        }
        public IQueryFilter StartsWith<T>(Func<IFunction, IFunction> function, T value)
        {
            _query.WhereStarts(function, value);
            return this;
        }

        public IQueryFilter EndsWith<T>(string column, T value)
        {
            _query.WhereEnds(column, value);
            return this;
        }
        public IQueryFilter EndsWith<T>(Func<IFunction, IFunction> function, T value)
        {
            _query.WhereEnds(function, value);
            return this;
        }

        public IQueryFilter Greater<T>(string column, T value)
        {
            _query.WhereGreater(column, value);
            return this;
        }
        public IQueryFilter Greater<T>(Func<IFunction, IFunction> function, T value)
        {
            _query.WhereGreater(function, value);
            return this;
        }

        public IQueryFilter GreaterOrEqual<T>(string column, T value)
        {
            _query.WhereGreaterOrEqual(column, value);
            return this;
        }
        public IQueryFilter GreaterOrEqual<T>(Func<IFunction, IFunction> function, T value)
        {
            _query.WhereGreaterOrEqual(function, value);
            return this;
        }

        public IQueryFilter Less<T>(string column, T value)
        {
            _query.WhereLess(column, value);
            return this;
        }
        public IQueryFilter Less<T>(Func<IFunction, IFunction> function, T value)
        {
            _query.WhereLess(function, value);
            return this;
        }

        public IQueryFilter LessOrEqual<T>(string column, T value)
        {
            _query.WhereLessOrEqual(column, value);
            return this;
        }
        public IQueryFilter LessOrEqual<T>(Func<IFunction, IFunction> function, T value)
        {
            _query.WhereLessOrEqual(function, value);
            return this;
        }

        public IQueryFilter And()
        {
            _query.And();
            return this;
        }

        public IQueryFilter And<T>(T value)
        {
            _query.And(value);
            return this;
        }

        public IQueryFilter Or()
        {
            _query.Or();
            return this;
        }

        public IQueryFilter Or<T>(T value)
        {
            _query.Or(value);
            return this;
        }
    }
}
