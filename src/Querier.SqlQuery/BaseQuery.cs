using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Functions;
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
    public class BaseQuery<TQuery> : BaseAbstractQuery<TQuery>, IBaseQuery<TQuery>
        where TQuery : IBaseQuery<TQuery>
    {
        public BaseQuery(IFunction functionFactory) : base(functionFactory) { }

        public virtual TQuery New()
        {
            return (TQuery)(object)new BaseQuery<TQuery>(_functionFactory);
        }

        public virtual TQuery Limit(int limit)
        {
            _limit = limit;
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
            var newQuery = query.Invoke(New());
            _table = new SqlTableQuery<TQuery>()
            {
                Query = newQuery,
                TableAs = tableAs
            };
            return (TQuery)(object)this;
        }

        public TQuery Select()
        {
            _whereColumn = new SqlColumn() { Column = "*" };

            _select.Add(new SqlSelect()
            {
                SqlColumn = new SqlColumn() { Column = "*" }
            });
            return (TQuery)(object)this;
        }
        public TQuery Select(string column, string? columnAs = null)
        {
            _whereColumn = new SqlColumn() { Column = column };

            _select.Add(new SqlSelect()
            {
                SqlColumn = new SqlColumn() { Column = column, ColumnAs = columnAs }
            });
            return (TQuery)(object)this;
        }
        public TQuery Select(string aggregation, string column, string? columnAs = null)
        {
            _whereColumn = new SqlColumn() { Column = column };

            _select.Add(new SqlSelectAggregation()
            {
                SqlColumnAggregation = new SqlColumnAggregation() { Column = column, ColumnAs = columnAs, Aggregation = aggregation }
            });
            return (TQuery)(object)this;
        }
        public TQuery SelectCount(string column, string? columnAs = null)
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
            var newQuery = query.Invoke(New());
            _select.Add(new SqlSelectQuery<TQuery>()
            {
                Query = newQuery,
                QueryAs = queryAs
            });
            return (TQuery)(object)this;
        }

        public TQuery SelectCase(string column, AbstractOperator @operator, string value, string defaulValue)
        {
            var sqlCase = new SqlCase() { ElseValue = defaulValue };
            sqlCase.AddCaseWhen(new SqlCaseWhen() { Operator = @operator, Value = value });

            _select.Add(new SqlSelectCase()
            {
                SqlCase = sqlCase
            });
            return (TQuery)(object)this;
        }

        public TQuery SelectDateFunction(IFunction function)
        {
            _select.Add(new SqlSelectFunction()
            {
                Function = function
            });
            return (TQuery)(object)this;
        }
        public TQuery SelectSecond(string column, string? columnAs = null)
        {
            return SelectDateFunction(_functionFactory.New().Second(column, columnAs));
        }
        public TQuery SelectMinute(string column, string? columnAs = null)
        {
            return SelectDateFunction(_functionFactory.New().Minute(column, columnAs));
        }
        public TQuery SelectHour(string column, string? columnAs = null)
        {
            return SelectDateFunction(_functionFactory.New().Hour(column, columnAs));
        }
        public TQuery SelectDay(string column, string? columnAs = null)
        {
            return SelectDateFunction(_functionFactory.New().Day(column, columnAs));
        }
        public TQuery SelectDate(string column, string? columnAs = null)
        {
            return SelectDateFunction(_functionFactory.New().Date(column, columnAs));
        }
        public TQuery SelectMonth(string column, string? columnAs = null)
        {
            return SelectDateFunction(_functionFactory.New().Month(column, columnAs));
        }
        public TQuery SelectYear(string column, string? columnAs = null)
        {
            return SelectDateFunction(_functionFactory.New().Year(column, columnAs));
        }

        public TQuery Distinct()
        {
            _distinct = true;
            return (TQuery)(object)this;
        }

        public TQuery WhereOperator(AbstractOperator @operator)
        {
            _where.Add(new SqlWhere(@operator.And(_whereAnd).Or(_whereOr)));

            _whereAnd = true;
            _whereOr = false;
            return (TQuery)(object)this;
        }
        public TQuery WhereOperator(string column, AbstractOperator @operator)
        {
            _whereColumn = new SqlColumn() { Column = column };
            _where.Add(new SqlWhere(@operator.And(_whereAnd).Or(_whereOr)));

            _whereAnd = true;
            _whereOr = false;
            return (TQuery)(object)this;
        }
        public TQuery Where(string column)
        {
            _whereColumn = new SqlColumn() { Column = column };
            return (TQuery)(object)this;
        }
        public TQuery Where(SqlColumn column)
        {
            _whereColumn = column;
            return (TQuery)(object)this;
        }
        public TQuery Where<T>(string column, T value)
        {
            return WhereOperator(column, new EqualOperator<T>() { Column = new SqlColumn() { Column = column }, Value = value });
        }
        public TQuery WhereEqual<T>(string column, T value)
        {
            return WhereOperator(column, new EqualOperator<T>() { Column = new SqlColumn() { Column = column }, Value = value });
        }
        public TQuery WhereEqual<T>(Func<IFunction, IFunction> function, T value)
        {
            return WhereOperator(new EqualOperator<T>() { Column = new SqlColumnFunction() { Function = function.Invoke(_functionFactory.New()) }, Value = value });
        }
        public TQuery Equal<T>(T value)
        {
            return WhereOperator(new EqualOperator<T>() { Column = _whereColumn, Value = value });
        }
        public TQuery WhereNotEqual<T>(string column, T value)
        {
            return WhereOperator(column, new NotEqualOperator<T>() { Column = new SqlColumn() { Column = column }, Value = value });
        }
        public TQuery NotEqual<T>(T value)
        {
            return WhereOperator(new NotEqualOperator<T>() { Column = _whereColumn, Value = value });
        }
        public TQuery WhereGreater<T>(string column, T value)
        {
            return WhereOperator(column, new GreaterThanOperator<T>() { Column = new SqlColumn() { Column = column }, Value = value });
        }
        public TQuery WhereGreater<T>(Func<IFunction, IFunction> function, T value)
        {
            return WhereOperator(new GreaterThanOperator<T>() { Column = new SqlColumnFunction() { Function = function.Invoke(_functionFactory.New()) }, Value = value });
        }
        public TQuery Greater<T>(T value)
        {
            return WhereOperator(new GreaterThanOperator<T>() { Column = _whereColumn, Value = value });
        }
        public TQuery WhereNotGreater<T>(string column, T value)
        {
            return WhereOperator(column, new GreaterThanOperator<T>() { Column = new SqlColumn() { Column = column }, Value = value }.Not());
        }
        public TQuery NotGreater<T>(T value)
        {
            return WhereOperator(new GreaterThanOperator<T>() { Column = _whereColumn, Value = value }.Not());
        }
        public TQuery WhereLess<T>(string column, T value)
        {
            return WhereOperator(column, new LessThanOperator<T>() { Column = new SqlColumn() { Column = column }, Value = value });
        }
        public TQuery WhereLess<T>(Func<IFunction, IFunction> function, T value)
        {
            return WhereOperator(new LessThanOperator<T>() { Column = new SqlColumnFunction() { Function = function.Invoke(_functionFactory.New()) }, Value = value });
        }
        public TQuery Less<T>(T value)
        {
            return WhereOperator(new LessThanOperator<T>() { Column = _whereColumn, Value = value });
        }
        public TQuery WhereNotLess<T>(string column, T value)
        {
            return WhereOperator(column, new LessThanOperator<T>() { Column = new SqlColumn() { Column = column }, Value = value }.Not());
        }
        public TQuery NotLess<T>(T value)
        {
            return WhereOperator(new LessThanOperator<T>() { Column = _whereColumn, Value = value }.Not());
        }
        public TQuery WhereGreaterOrEqual<T>(string column, T value)
        {
            return WhereOperator(column, new GreaterThanOrEqualOperator<T>() { Column = new SqlColumn() { Column = column }, Value = value });
        }
        public TQuery WhereGreaterOrEqual<T>(Func<IFunction, IFunction> function, T value)
        {
            return WhereOperator(new GreaterThanOrEqualOperator<T>() { Column = new SqlColumnFunction() { Function = function.Invoke(_functionFactory.New()) }, Value = value });
        }
        public TQuery GreaterOrEqual<T>(T value)
        {
            return WhereOperator(new GreaterThanOrEqualOperator<T>() { Column = _whereColumn, Value = value });
        }
        public TQuery WhereNotGreaterOrEqual<T>(string column, T value)
        {
            return WhereOperator(column, new GreaterThanOrEqualOperator<T>() { Column = new SqlColumn() { Column = column }, Value = value }.Not());
        }
        public TQuery NotGreaterOrEqual<T>(T value)
        {
            return WhereOperator(new GreaterThanOrEqualOperator<T>() { Column = _whereColumn, Value = value }.Not());
        }
        public TQuery WhereLessOrEqual<T>(string column, T value)
        {
            return WhereOperator(column, new LessThanOrEqualOperator<T>() { Column = new SqlColumn() { Column = column }, Value = value });
        }
        public TQuery WhereLessOrEqual<T>(Func<IFunction, IFunction> function, T value)
        {
            return WhereOperator(new LessThanOrEqualOperator<T>() { Column = new SqlColumnFunction() { Function = function.Invoke(_functionFactory.New()) }, Value = value });
        }
        public TQuery LessOrEqual<T>(T value)
        {
            return WhereOperator(new LessThanOrEqualOperator<T>() { Column = _whereColumn, Value = value });
        }
        public TQuery WhereNotLessOrEqual<T>(string column, T value)
        {
            return WhereOperator(column, new LessThanOrEqualOperator<T>() { Column = new SqlColumn() { Column = column }, Value = value }.Not());
        }
        public TQuery NotLessOrEqual<T>(T value)
        {
            return WhereOperator(new LessThanOrEqualOperator<T>() { Column = _whereColumn, Value = value }.Not());
        }
        public TQuery WhereBetween<T>(string column, T value, T secondValue)
        {
            return WhereOperator(column, new BetweenOperator() { Column = new SqlColumn() { Column = column }, Value = value, SecondValue = secondValue });
        }
        public TQuery Between<T>(T value, T secondValue)
        {
            return WhereOperator(new BetweenOperator() { Column = _whereColumn, Value = value, SecondValue = secondValue });
        }
        public TQuery WhereNotBetween<T>(string column, T value, T secondValue)
        {
            return WhereOperator(column, new BetweenOperator() { Column = new SqlColumn() { Column = column }, Value = value, SecondValue = secondValue }.Not());
        }
        public TQuery NotBetween<T>(T value, T secondValue)
        {
            return WhereOperator(new BetweenOperator() { Column = _whereColumn, Value = value, SecondValue = secondValue }.Not());
        }
        public TQuery WhereIn<T>(string column, IEnumerable<T> value)
        {
            return WhereOperator(column, new InOperator() { Column = new SqlColumn() { Column = column }, Value = value });
        }
        public TQuery WhereIn<T>(Func<IFunction, IFunction> function, IEnumerable<T> value)
        {
            return WhereOperator(new InOperator() { Column = new SqlColumnFunction() { Function = function.Invoke(_functionFactory.New()) }, Value = value });
        }
        public TQuery In<T>(IEnumerable<T> value)
        {
            return WhereOperator(new InOperator() { Column = _whereColumn, Value = value });
        }
        public TQuery WhereNotIn<T>(string column, IEnumerable<T> value)
        {
            return WhereOperator(column, new InOperator() { Column = new SqlColumn() { Column = column }, Value = value }.Not());
        }
        public TQuery NotIn<T>(IEnumerable<T> value)
        {
            return WhereOperator(new InOperator() { Column = _whereColumn, Value = value }.Not());
        }
        public TQuery WhereLike<T>(string column, T value)
        {
            return WhereOperator(column, new LikeOperator() { Column = new SqlColumn() { Column = column }, Value = value, LikeStarts = "%", LikeEnds = "%" });
        }
        public TQuery WhereLike<T>(Func<IFunction, IFunction> function, T value)
        {
            return WhereOperator(new LikeOperator() { Column = new SqlColumnFunction() { Function = function.Invoke(_functionFactory.New()) }, Value = value, LikeStarts = "%", LikeEnds = "%" });
        }
        public TQuery Like<T>(T value)
        {
            return WhereOperator(new LikeOperator() { Column = _whereColumn, Value = value, LikeStarts = "%", LikeEnds = "%" });
        }
        public TQuery WhereNotLike<T>(string column, T value)
        {
            return WhereOperator(column, new LikeOperator() { Column = new SqlColumn() { Column = column }, Value = value, LikeStarts = "%", LikeEnds = "%" }.Not());
        }
        public TQuery NotLike<T>(T value)
        {
            return WhereOperator(new LikeOperator() { Column = _whereColumn, Value = value, LikeStarts = "%", LikeEnds = "%" }.Not());
        }

        public TQuery WhereStarts<T>(string column, T value)
        {
            return WhereOperator(column, new LikeOperator() { Column = new SqlColumn() { Column = column }, Value = value, LikeStarts = "", LikeEnds = "%" });
        }
        public TQuery WhereStarts<T>(Func<IFunction, IFunction> function, T value)
        {
            return WhereOperator(new LikeOperator() { Column = new SqlColumnFunction() { Function = function.Invoke(_functionFactory.New()) }, Value = value, LikeStarts = "", LikeEnds = "%" });
        }
        public TQuery WhereNotStarts<T>(string column, T value)
        {
            return WhereOperator(column, new LikeOperator() { Column = new SqlColumn() { Column = column }, Value = value, LikeStarts = "", LikeEnds = "%" }.Not());
        }
        public TQuery Starts<T>(T value)
        {
            return WhereOperator(new LikeOperator() { Column = _whereColumn, Value = value, LikeStarts = "", LikeEnds = "%" });
        }
        public TQuery NotStarts<T>(T value)
        {
            return WhereOperator(new LikeOperator() { Column = _whereColumn, Value = value, LikeStarts = "", LikeEnds = "%" }.Not());
        }

        public TQuery WhereEnds<T>(string column, T value)
        {
            return WhereOperator(column, new LikeOperator() { Column = new SqlColumn() { Column = column }, Value = value, LikeStarts = "%", LikeEnds = "" });
        }
        public TQuery WhereEnds<T>(Func<IFunction, IFunction> function, T value)
        {
            return WhereOperator(new LikeOperator() { Column = new SqlColumnFunction() { Function = function.Invoke(_functionFactory.New()) }, Value = value, LikeStarts = "%", LikeEnds = "" });
        }
        public TQuery WhereNotEnds<T>(string column, T value)
        {
            return WhereOperator(column, new LikeOperator() { Column = new SqlColumn() { Column = column }, Value = value, LikeStarts = "%", LikeEnds = "" }.Not());
        }
        public TQuery Ends<T>(T value)
        {
            return WhereOperator(new LikeOperator() { Column = _whereColumn, Value = value, LikeStarts = "%", LikeEnds = "" });
        }
        public TQuery NotEnds<T>(T value)
        {
            return WhereOperator(new LikeOperator() { Column = _whereColumn, Value = value, LikeStarts = "%", LikeEnds = "" }.Not());
        }
        public TQuery WhereNull(string column)
        {
            return WhereOperator(column, new IsNullOperator() { Column = new SqlColumn() { Column = column } });
        }
        public TQuery Null()
        {
            return WhereOperator(new IsNullOperator() { Column = _whereColumn });
        }
        public TQuery WhereNotNull(string column)
        {
            return WhereOperator(column, new IsNullOperator() { Column = new SqlColumn() { Column = column } }.Not());
        }
        public TQuery NotNull()
        {
            return WhereOperator(new IsNullOperator() { Column = _whereColumn }.Not());
        }


        public TQuery And()
        {
            _whereAnd = true;
            _whereOr = false;
            return (TQuery)(object)this;
        }

        public TQuery And<T>(T value)
        {
            var comparison = _where.Where(e => e.Operator.IsOperatorType<AbstractComparisonOperator<T>>()).LastOrDefault();
            if (comparison == null) return (TQuery)(object)this;

            var clonedComparison = comparison.Clone();
            var clonedComparisonOperator = clonedComparison.Operator.And() as AbstractComparisonOperator<T>;
            clonedComparisonOperator.Value = value;

            return WhereOperator(clonedComparisonOperator);
        }
        public TQuery And<T>(T value, T? secondValue)
        {
            var comparison = _where.Where(e => e.Operator.IsOperatorType<AbstractComparisonOperator<T>>()).LastOrDefault();
            if (comparison == null) return (TQuery)(object)this;

            var clonedComparison = comparison.Clone();
            var clonedComparisonOperator = clonedComparison.Operator.And() as AbstractComparisonOperator<T>;
            clonedComparisonOperator.Value = value;

            return WhereOperator(clonedComparisonOperator);
        }

        public TQuery Or()
        {
            _whereAnd = false;
            _whereOr = true;
            return (TQuery)(object)this;
        }
        public TQuery Or<T>(T value)
        {
            var comparison = _where.Where(e => e.Operator.IsOperatorType<AbstractComparisonOperator<T>>()).LastOrDefault();
            if (comparison == null) return (TQuery)(object)this;

            var clonedComparison = comparison.Clone();
            var clonedComparisonOperator = clonedComparison.Operator.Or() as AbstractComparisonOperator<T>;
            clonedComparisonOperator.Value = value;

            return WhereOperator(clonedComparisonOperator);
        }
        public TQuery Or<T>(T value, T? secondValue)
        {
            var comparison = _where.Where(e => e.Operator.IsOperatorType<AbstractComparisonOperator<T>>()).LastOrDefault();
            if (comparison == null) return (TQuery)(object)this;

            var clonedComparison = comparison.Clone();
            var clonedComparisonOperator = clonedComparison.Operator.Or() as AbstractComparisonOperator<T>;
            clonedComparisonOperator.Value = value;

            return WhereOperator(clonedComparisonOperator);
        }

        public TQuery GroupBy(string column)
        {
            _groupBy.Add(new SqlGroupBy()
            {
                Column = column
            });

            return (TQuery)(object)this;
        }
        public TQuery GroupBy(Func<IFunction, IFunction> function)
        {
            _groupBy.Add(new SqlGroupByFunction()
            {
                Column = string.Empty,
                Function = function.Invoke(_functionFactory.New())
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
            SqlParameters = new Dictionary<string, object>();
            NameParameters = new Dictionary<string, string>();

            var result = new SqlQueryResult();

            var queryTz = CompileTokens(result);
            result.SqlParameters = CompileSqlParameters(result);
            result.NameParameters = CompileNameParameters(result);

            result.Sql = queryTz.Build(" ");
            result = CompileSql(result);

            return result;
        }
    }
}
