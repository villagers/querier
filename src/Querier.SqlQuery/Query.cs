using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Models;
using Querier.SqlQuery.Operators;
using Querier.SqlQuery.Tokenizers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Querier.SqlQuery
{
    public class Query<TQuery> : IQuery<TQuery> where TQuery : IQuery<TQuery>, new()
    {
        TQuery _parent;

        public SqlTable<TQuery> _table;
        protected readonly List<SqlSelect> _select;
        protected readonly List<SqlWhere> _where;
        protected readonly List<SqlGroupBy> _groupBy;
        protected readonly List<SqlOrderBy> _orderBy;

        public Dictionary<string, object> SqlParameters;
        public Dictionary<string, string> NameParameters;

        protected bool _whereAnd = false;
        protected bool _whereOr = false;
        protected string _whereColumn = string.Empty;

        protected bool _distinct = false;

        protected virtual string NameParameterPlaceholder { get; set; } = "@n";
        protected virtual string SqlParameterPlaceholder { get; set; } = "@p";

        public Query()
        {
            _table = new SqlTable<TQuery>();
            _select = new List<SqlSelect>();
            _where = new List<SqlWhere>();
            _groupBy = new List<SqlGroupBy>();
            _orderBy = new List<SqlOrderBy>();

            SqlParameters = new Dictionary<string, object>();
            NameParameters = new Dictionary<string, string>();
        }

        public TQuery New()
        {
            return new TQuery();
        }
        public TQuery Parent(TQuery parentQuery)
        {
            _parent = parentQuery;
            return (TQuery)(object)this;
        }

        public TQuery From(string table, string? tableAs = null)
        {
            _table = new SqlTable<TQuery>()
            {
                Table = table,
                TableAs = tableAs
            };

            return (TQuery)(object)this;
        }
        public TQuery From(Func<TQuery, TQuery> query, string? tableAs = null)
        {
            var newQuery = query.Invoke(new TQuery());
            _table = new SqlTableQuery<TQuery>()
            {
                Query = newQuery,
                TableAs = tableAs
            };
            return (TQuery)(object)this;
        }

        public TQuery Select()
        {
            _whereColumn = "*";

            _select.Add(new SqlSelect()
            {
                Column = "*"
            });
            return (TQuery)(object)this;
        }
        public TQuery Select(string column, string? columnAs = null)
        {
            RemoveSelect();
            _whereColumn = column;

            _select.Add(new SqlSelect()
            {
                Column = column,
                ColumnAs = columnAs
            });
            return (TQuery)(object)this;
        }
        public TQuery Select(string aggregation, string column, string? columnAs = null)
        {
            RemoveSelect();
            _whereColumn = column;

            _select.Add(new SqlSelectAggregation()
            {
                Column = column,
                ColumnAs = columnAs,
                Aggregation = aggregation
            });
            return (TQuery)(object)this;
        }
        public TQuery SelectCount(string column = "*", string? columnAs = null)
        {
            return Select("count", column, columnAs);
        }
        public TQuery SelectSum(string column, string? columnAs = null)
        {
            return Select("sum", column, columnAs);
        }
        public TQuery SelectAvg(string column, string? columnAs = null)
        {
            return Select("avg", column, columnAs);
        }
        public TQuery SelectMin(string column, string? columnAs = null)
        {
            return Select("min", column, columnAs);
        }
        public TQuery SelectMax(string column, string? columnAs = null)
        {
            return Select("max", column, columnAs);
        }
        public TQuery Select(Func<TQuery, TQuery> query, string? queryAs = null)
        {
            var newQuery = query.Invoke(new TQuery());
            _select.Add(new SqlSelectQuery<TQuery>()
            {
                Query = newQuery,
                QueryAs = queryAs
            });
            return (TQuery)(object)this;
        }
        public TQuery Distinct()
        {
            _distinct = true;
            return (TQuery)(object)this;
        }
        private void RemoveSelect()
        {
            if (_select.Count <= 0) return;
            if (string.IsNullOrEmpty(_whereColumn)) return;
            if (_whereColumn != "*") return;

            var select = _select.Where(e => e.Column == "*").FirstOrDefault();
            if (select != null)
            {
                _select.Remove(select);
            }
        }
        public TQuery Where(string column, AbstractOperator @operator)
        {
            _whereColumn = column;
            _where.Add(new SqlWhere(column, @operator.And(_whereAnd).Or(_whereOr)));

            _whereAnd = true;
            _whereOr = false;
            return (TQuery)(object)this;
        }
        public TQuery Where(string column)
        {
            _whereColumn = column;
            return (TQuery)(object)this;
        }
        public TQuery Where(string column, object value)
        {
            return Where(column, new EqualOperator() { Column = column, Value = value });
        }
        public TQuery WhereEqual(string column, object value)
        {
            return Where(column, new EqualOperator() { Column = column, Value = value });
        }
        public TQuery Equal(object value)
        {
            return Where(_whereColumn, new EqualOperator() { Column = _whereColumn, Value = value });
        }
        public TQuery WhereNotEqual(string column, object value)
        {
            return Where(column, new NotEqualOperator() { Column = column, Value = value });
        }
        public TQuery NotEqual(object value)
        {
            return Where(_whereColumn, new NotEqualOperator() { Column = _whereColumn, Value = value });
        }
        public TQuery WhereGreater(string column, object value)
        {
            return Where(column, new GreaterThanOperator() { Column = column, Value = value });
        }
        public TQuery Greater(object value)
        {
            return Where(_whereColumn, new GreaterThanOperator() { Column = _whereColumn, Value = value });
        }
        public TQuery WhereNotGreater(string column, object value)
        {
            return Where(column, new GreaterThanOperator() { Column = column, Value = value }.Not());
        }
        public TQuery NotGreater(object value)
        {
            return Where(_whereColumn, new GreaterThanOperator() { Column = _whereColumn, Value = value }.Not());
        }
        public TQuery WhereLess(string column, object value)
        {
            return Where(column, new LessThanOperator() { Column = column, Value = value });
        }
        public TQuery Less(object value)
        {
            return Where(_whereColumn, new LessThanOperator() { Column = _whereColumn, Value = value });
        }
        public TQuery WhereNotLess(string column, object value)
        {
            return Where(column, new LessThanOperator() { Column = column, Value = value }.Not());
        }
        public TQuery NotLess(object value)
        {
            return Where(_whereColumn, new LessThanOperator() { Column = _whereColumn, Value = value }.Not());
        }
        public TQuery WhereGreaterOrEqual(string column, object value)
        {
            return Where(column, new GreaterThanOrEqualOperator() { Column = column, Value = value });
        }
        public TQuery GreaterOrEqual(object value)
        {
            return Where(_whereColumn, new GreaterThanOrEqualOperator() { Column = _whereColumn, Value = value });
        }
        public TQuery WhereNotGreaterOrEqual(string column, object value)
        {
            return Where(column, new GreaterThanOrEqualOperator() { Column = column, Value = value }.Not());
        }
        public TQuery NotGreaterOrEqual(object value)
        {
            return Where(_whereColumn, new GreaterThanOrEqualOperator() { Column = _whereColumn, Value = value }.Not());
        }
        public TQuery WhereLessOrEqual(string column, object value)
        {
            return Where(column, new LessThanOrEqualOperator() { Column = column, Value = value });
        }
        public TQuery LessOrEqual(object value)
        {
            return Where(_whereColumn, new LessThanOrEqualOperator() { Column = _whereColumn, Value = value });
        }
        public TQuery WhereNotLessOrEqual(string column, object value)
        {
            return Where(column, new LessThanOrEqualOperator() { Column = column, Value = value }.Not());
        }
        public TQuery NotLessOrEqual(object value)
        {
            return Where(_whereColumn, new LessThanOrEqualOperator() { Column = _whereColumn, Value = value }.Not());
        }
        public TQuery WhereBetween(string column, object value, object secondValue)
        {
            return Where(column, new BetweenOperator() { Column = column, Value = value, SecondValue = secondValue });
        }
        public TQuery Between(object value, object secondValue)
        {
            return Where(_whereColumn, new BetweenOperator() { Column = _whereColumn, Value = value, SecondValue = secondValue });
        }
        public TQuery WhereNotBetween(string column, object value, object secondValue)
        {
            return Where(column, new BetweenOperator() { Column = column, Value = value, SecondValue = secondValue }.Not());
        }
        public TQuery NotBetween(object value, object secondValue)
        {
            return Where(_whereColumn, new BetweenOperator() { Column = _whereColumn, Value = value, SecondValue = secondValue }.Not());
        }
        public TQuery WhereIn(string column, object value)
        {
            return Where(column, new InOperator() { Column = column, Value = value });
        }
        public TQuery In(object value)
        {
            return Where(_whereColumn, new InOperator() { Column = _whereColumn, Value = value });
        }
        public TQuery WhereNotIn(string column, object value)
        {
            return Where(column, new InOperator() { Column = column, Value = value }.Not());
        }
        public TQuery NotIn(object value)
        {
            return Where(_whereColumn, new InOperator() { Column = _whereColumn, Value = value }.Not());
        }
        public TQuery WhereLike(string column, object value)
        {
            return Where(column, new LikeOperator() { Column = column, Value = value });
        }
        public TQuery Like(object value)
        {
            return Where(_whereColumn, new LikeOperator() { Column = _whereColumn, Value = value });
        }
        public TQuery WhereNotLike(string column, object value)
        {
            return Where(column, new LikeOperator() { Column = column, Value = value }.Not());
        }
        public TQuery NotLike(object value)
        {
            return Where(_whereColumn, new LikeOperator() { Column = _whereColumn, Value = value }.Not());
        }

        public TQuery WhereStarts(string column, object value)
        {
            return Where(column, new LikeOperator() { Column = column, Value = value, Pattern = "{0}%" });
        }
        public TQuery WhereNotStarts(string column, object value)
        {
            return Where(column, new LikeOperator() { Column = column, Value = value, Pattern = "{0}%" }.Not());
        }
        public TQuery Starts(object value)
        {
            return Where(_whereColumn, new LikeOperator() { Column = _whereColumn, Value = value, Pattern = "{0}%" });
        }
        public TQuery NotStarts(object value)
        {
            return Where(_whereColumn, new LikeOperator() { Column = _whereColumn, Value = value, Pattern = "{0}%" }.Not());
        }

        public TQuery WhereEnds(string column, object value)
        {
            return Where(column, new LikeOperator() { Column = column, Value = value, Pattern = "%{0}" });
        }
        public TQuery WhereNotEnds(string column, object value)
        {
            return Where(column, new LikeOperator() { Column = column, Value = value, Pattern = "%{0}" }.Not());
        }
        public TQuery Ends(object value)
        {
            return Where(_whereColumn, new LikeOperator() { Column = _whereColumn, Value = value, Pattern = "%{0}" });
        }
        public TQuery NotEnds(object value)
        {
            return Where(_whereColumn, new LikeOperator() { Column = _whereColumn, Value = value, Pattern = "%{0}" }.Not());
        }

        public TQuery WhereAll(string column, string @operator, Func<TQuery, TQuery> query)
        {
            var newQuery = query.Invoke(new TQuery());
            return Where(column, new AllOperator<TQuery>() { Column = column, Query = newQuery, Operator = @operator });
        }
        public TQuery All(string @operator, Func<TQuery, TQuery> query)
        {
            var newQuery = query.Invoke(new TQuery());
            return Where(_whereColumn, new AllOperator<TQuery>() { Column = _whereColumn, Query = newQuery, Operator = @operator });
        }
        public TQuery WhereAny(string column, string @operator, Func<TQuery, TQuery> query)
        {
            var newQuery = query.Invoke(new TQuery());
            return Where(column, new AnyOperator<TQuery>() { Column = column, Query = newQuery, Operator = @operator });
        }
        public TQuery Any(string @operator, Func<TQuery, TQuery> query)
        {
            var newQuery = query.Invoke(new TQuery());
            return Where(_whereColumn, new AnyOperator<TQuery>() { Column = _whereColumn, Query = newQuery, Operator = @operator });
        }
        public TQuery WhereExists(Func<TQuery, TQuery> query)
        {
            var newQuery = query.Invoke(new TQuery());
            return Where("", new ExistsOperator<TQuery>() { Query = newQuery });
        }
        public TQuery WhereNotExists(Func<TQuery, TQuery> query)
        {
            var newQuery = query.Invoke(new TQuery());
            return Where("", new ExistsOperator<TQuery>() { Query = newQuery }.Not());
        }
        public TQuery WhereNull(string column)
        {
            return Where(column, new IsNullOperator() { Column = column });
        }
        public TQuery Null()
        {
            return Where(_whereColumn, new IsNullOperator() { Column = _whereColumn });
        }
        public TQuery WhereNotNull(string column)
        {
            return Where(column, new IsNullOperator() { Column = column }.Not());
        }
        public TQuery NotNull()
        {
            return Where(_whereColumn, new IsNullOperator() { Column = _whereColumn }.Not());
        }



        public TQuery And()
        {
            _whereAnd = true;
            _whereOr = false;
            return (TQuery)(object)this;
        }
        public TQuery And(object value, object? secondValue = null)
        {
            var comparison = _where.Where(e => e.Operator.IsOperatorType<AbstractComparisonOperator>()).LastOrDefault();
            if (comparison == null) return (TQuery)(object)this;

            var clonedComparison = comparison.Clone();
            var clonedComparisonOperator = clonedComparison.Operator.And() as AbstractComparisonOperator;
            clonedComparisonOperator.Value = value;

            return Where(clonedComparisonOperator.Column, clonedComparisonOperator);
        }

        public TQuery Or()
        {
            _whereAnd = false;
            _whereOr = true;
            return (TQuery)(object)this;
        }
        public TQuery Or(object value, object? secondValue = null)
        {
            var comparison = _where.Where(e => e.Operator.IsOperatorType<AbstractComparisonOperator>()).LastOrDefault();
            if (comparison == null) return (TQuery)(object)this;

            var clonedComparison = comparison.Clone();
            var clonedComparisonOperator = clonedComparison.Operator.Or() as AbstractComparisonOperator;
            clonedComparisonOperator.Value = value;

            return Where(clonedComparisonOperator.Column, clonedComparisonOperator);
        }

        public TQuery GroupBy(string column)
        {
            _groupBy.Add(new SqlGroupBy()
            {
                Column = column
            });

            return (TQuery)(object)this;
        }

        public TQuery OrderBy(string column, string? order = "asc")
        {
            _orderBy.Add(new SqlOrderBy()
            {
                Column = column,
                Order = order
            });

            return (TQuery)(object)this;
        }

        public virtual SqlQueryResult Compile()
        {
            var result = new SqlQueryResult();

            var queryTz = new SqlTokenizer();

            var selectQuery = CreateSelect();
            var tableQuery = CreateTable();
            var whereQuery = CreateWhere();
            var groupByQuery = CreateGroupBy();
            var orderByQuery = CreateOrderBy();

            queryTz.AddToken(selectQuery.Sql);

            foreach (var param in selectQuery.SqlParameters)
            {
                result.SqlParameters.Add(param.Key, param.Value);
            }
            foreach (var param in selectQuery.NameParameters)
            {
                result.NameParameters.Add(param.Key, param.Value);
            }

            queryTz.AddToken(tableQuery.Sql);
            foreach (var param in tableQuery.SqlParameters)
            {
                result.SqlParameters.Add(param.Key, param.Value);
            }
            foreach (var param in tableQuery.NameParameters)
            {
                result.NameParameters.Add(param.Key, param.Value);
            }

            queryTz.AddToken(whereQuery.Sql);
            foreach (var param in whereQuery.SqlParameters)
            {
                result.SqlParameters.Add(param.Key, param.Value);
            }

            queryTz.AddToken(groupByQuery.Sql);
            foreach (var param in groupByQuery.SqlParameters)
            {
                result.SqlParameters.Add(param.Key, param.Value);
            }

            queryTz.AddToken(orderByQuery.Sql);
            foreach (var param in orderByQuery.SqlParameters)
            {
                result.SqlParameters.Add(param.Key, param.Value);
            }

            result.Sql = queryTz.Build(" ");

            return result;

        }


        public virtual SqlQueryResult CreateTable()
        {
            var result = new SqlQueryResult();

            var tableTz = new SqlTokenizer().AddToken("from");

            var tableCompiled = _table.Compile();
            var tableCompiledSql = tableCompiled.Sql;

            foreach (var parameter in tableCompiled.NameParameters.Reverse())
            {
                var nameCount = NameParameters.Count;
                var newPlaceholder = $"{NameParameterPlaceholder}{nameCount}";

                tableCompiledSql = tableCompiledSql.Replace(parameter.Key, newPlaceholder);

                NameParameters.Add(newPlaceholder, parameter.Value);
                result.NameParameters.Add(newPlaceholder, parameter.Value);
            }

            tableTz.AddToken(tableCompiledSql);
            result.Sql = tableTz.Build(" ");
            return result;
        }
        public virtual SqlQueryResult CreateSelect()
        {
            var result = new SqlQueryResult();

            var selectTz = new SqlTokenizer().AddToken("select");
            if (_select.Count <= 0)
            {
                result.Sql = selectTz.AddToken("*").Build(" ");
                return result;
            }

            if (_distinct)
            {
                selectTz.AddToken("distinct");
            }

            selectTz.AddToken(tz =>
            {
                foreach (var select in _select)
                {
                    var selectCompiled = select.Compile();
                    var selectCompiledSql = selectCompiled.Sql;
                    foreach (var parameter in selectCompiled.SqlParameters)
                    {
                        var count = SqlParameters.Count;

                        var name = parameter.Key;
                        var newName = $"{SqlParameterPlaceholder}{count}";

                        selectCompiledSql = selectCompiledSql.Replace(name, newName);

                        SqlParameters.Add(newName, parameter.Value);
                        result.SqlParameters.Add(newName, parameter.Value);
                    }

                    foreach (var parameter in selectCompiled.NameParameters.Reverse())
                    {
                        var nameCount = NameParameters.Count;
                        var newPlaceholder = $"{NameParameterPlaceholder}{nameCount}";

                        selectCompiledSql = selectCompiledSql.Replace(parameter.Key, newPlaceholder);

                        NameParameters.Add(newPlaceholder, parameter.Value);
                        result.NameParameters.Add(newPlaceholder, parameter.Value);
                    }

                    tz.AddToken(selectCompiledSql);
                }

                return tz;
            }, ", ");


            result.SqlTokenizer = selectTz;
            result.Sql = selectTz.Build(" ");
            return result;
        }
        public virtual SqlQueryResult CreateWhere()
        {
            var result = new SqlQueryResult();

            if (_where.Count <= 0) return result;

            var whereTz = new SqlTokenizer().AddToken("where");

            foreach (var where in _where)
            {
                var whereCompiled = where.Operator.Compile();
                var whereCompiledSql = whereCompiled.Sql;


                foreach (var parameter in whereCompiled.SqlParameters)
                {
                    var count = SqlParameters.Count;

                    var name = parameter.Key;
                    var newName = $"{SqlParameterPlaceholder}{count}";

                    whereCompiledSql = whereCompiledSql.Replace(name, newName);

                    SqlParameters.Add(newName, parameter.Value);
                    result.SqlParameters.Add(newName, parameter.Value);
                }

                foreach (var parameter in whereCompiled.NameParameters.Reverse())
                {
                    var nameCount = NameParameters.Count;
                    var newPlaceholder = $"{NameParameterPlaceholder}{nameCount}";

                    whereCompiledSql = whereCompiledSql.Replace(parameter.Key, newPlaceholder);

                    NameParameters.Add(newPlaceholder, parameter.Value);
                    result.NameParameters.Add(newPlaceholder, parameter.Value);
                }

                whereTz.AddToken(whereCompiledSql);
            }
            result.Sql = whereTz.Build();
            return result;
        }
        public virtual SqlQueryResult CreateGroupBy()
        {
            var result = new SqlQueryResult();

            if (_groupBy.Count <= 0) return result;

            var groupTz = new SqlTokenizer().AddToken("group").AddToken("by");

            groupTz.AddToken(e =>
            {
                foreach (var group in _groupBy)
                {
                    var count = NameParameters.Count;
                    var newName = $"{NameParameterPlaceholder}{count}";

                    NameParameters.Add(newName, group.Column);
                    result.NameParameters.Add(newName, group.Column);

                    e.AddToken(newName);
                }
                return e;
            }, ", ");

            result.Sql = groupTz.Build();
            return result;
        }
        public virtual SqlQueryResult CreateOrderBy()
        {
            var result = new SqlQueryResult();

            if (_orderBy.Count <= 0) return result;

            var orderTz = new SqlTokenizer().AddToken("order").AddToken("by");

            orderTz.AddToken(e =>
            {
                foreach (var order in _orderBy)
                {
                    var count = NameParameters.Count;
                    var newName = $"{NameParameterPlaceholder}{count}";

                    NameParameters.Add(newName, order.Column);
                    result.NameParameters.Add(newName, order.Column);
                    e.AddToken(e => e.AddToken(s => s.AddToken(newName).AddToken(order.Order), " "));
                }
                return e;
            }, ", ");

            result.Sql = orderTz.Build();
            return result;
        }
    }
}
