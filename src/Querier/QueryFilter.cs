using Querier.Interfaces;
using Querier.SqlQuery;

namespace Querier
{
    public class QueryFilter : IQueryFilter
    {
        private readonly string _column;
        private readonly IDuckDBQueryBuilder _query;

        public QueryFilter(IDuckDBQueryBuilder query)
        {
            _query = query;
        }
        public QueryFilter(IDuckDBQueryBuilder query, string column)
        {
            _query = query;
            _column = column;

            _query.Where(column);
        }
        public IQueryFilter New()
        {
            return new QueryFilter(_query);
        }

        public IQueryFilter In<T>(IEnumerable<T> value)
        {
            _query.In(value);
            return this;
        }
        public IQueryFilter In(string rawSql)
        {
            _query.In(rawSql);
            return this;
        }

        public IQueryFilter Equal<T>(T value)
        {
            _query.Equal(value);
            return this;
        }
        public IQueryFilter Equal(string rawSql)
        {
            _query.Equal(rawSql);
            return this;
        }

        public IQueryFilter Contains<T>(T value)
        {
            _query.Like(value);
            return this;
        }
        public IQueryFilter Contains(string rawSql)
        {
            _query.Like(rawSql);
            return this;
        }

        public IQueryFilter StartsWith<T>(T value)
        {
            _query.Starts(value);
            return this;
        }
        public IQueryFilter StartsWith(string rawSql)
        {
            _query.Starts(rawSql);
            return this;
        }

        public IQueryFilter EndsWith<T>(T value)
        {
            _query.Ends(value);
            return this;
        }
        public IQueryFilter EndsWith(string rawSql)
        {
            _query.Ends(rawSql);
            return this;
        }

        public IQueryFilter Greater<T>(T value)
        {
            _query.Greater(value);
            return this;
        }
        public IQueryFilter Greater(string rawSql)
        {
            _query.Greater(rawSql);
            return this;
        }

        public IQueryFilter GreaterOrEqual<T>(T value)
        {
            _query.GreaterOrEqual(value);
            return this;
        }
        public IQueryFilter GreaterOrEqual(string rawSql)
        {
            _query.GreaterOrEqual(rawSql);
            return this;
        }

        public IQueryFilter Less<T>(T value)
        {
            _query.Less(value);
            return this;
        }
        public IQueryFilter Less(string rawSql)
        {
            _query.Less(rawSql);
            return this;
        }

        public IQueryFilter LessOrEqual<T>(T value)
        {
            _query.LessOrEqual(value);
            return this;
        }
        public IQueryFilter LessOrEqual(string rawSql)
        {
            _query.LessOrEqual(rawSql);
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
