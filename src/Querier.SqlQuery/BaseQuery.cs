﻿using Querier.SqlQuery.Functions;
using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Models;
using Querier.SqlQuery.Operators;

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

        public TQuery FromRaw(string sql)
        {
            _table = new SqlTableRaw()
            {
                RawSql = sql
            };

            return (TQuery)(object)this;
        }
        public TQuery From(string table, string? tableAs = null)
        {
            _table = new SqlTable<TQuery>()
            {
                Table = table,
                TableAs = tableAs ?? table
            };

            return (TQuery)(object)this;
        }
        public TQuery From(Func<TQuery, TQuery> query, string tableAs)
        {
            var newQuery = query.Invoke(New());
            _table = new SqlTableQuery<TQuery>()
            {
                Query = newQuery,
                TableAs = tableAs
            };
            return (TQuery)(object)this;
        }

        public TQuery With(string name, Func<TQuery, TQuery> query)
        {
            var newQuery = query.Invoke(New());
            _cte.Add(new SqlCte<TQuery>()
            {
                Name = name,
                Query = newQuery,
                Recursive = false
            });
            return (TQuery)(object)this;
        }
        public TQuery WithRecursive(string name, Func<TQuery, TQuery> query)
        {
            var newQuery = query.Invoke(New());
            _cte.Add(new SqlCte<TQuery>()
            {
                Name = name,
                Query = newQuery,
                Recursive = true
            });
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

        public TQuery SelectCase<T>(string column, T value, object thenValue, object? elseValue = null)
        {
            var sqlCase = new SqlCase() { ElseValue = elseValue };
            sqlCase.AddCaseWhen(new SqlCaseWhen()
            {
                Operator = new EqualOperator<T>()
                {
                    Column = new SqlColumn()
                    {
                        Column = column
                    },
                    Value = value
                },
                Value = thenValue
            });

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
        public TQuery SelectRaw(string sql, string? sqlAs = null)
        {
            _select.Add(new SqlSelectRaw()
            {
                RawSql = sql,
                RawSqlAs = sqlAs
            });
            return (TQuery)(object)this;
        }
        public TQuery SelectCoalesce<T>(Func<TQuery, TQuery> query, T value, string? queryAs = null)
        {
            var newQuery = query.Invoke(New());
            _select.Add(new SqlSelectQuery<TQuery>()
            {
                Query = newQuery,
                QueryAs = queryAs,
                Function = "coalesce",
                FunctionParameters = new List<object> { value }
            });
            return (TQuery)(object)this;
        }
        public TQuery JoinRaw(string referenceTable, string rawSql)
        {
            _join.Add(new SqlJoinRaw()
            {
                RawSql = rawSql,
                RefenreceTable = referenceTable
            });
            return (TQuery)(object)this;
        }
        public TQuery Join(string column, string referenceTable, string referenceColumn)
        {
            _join.Add(new SqlJoin()
            {
                RefenreceTable = referenceTable,
            }.On(column, referenceColumn));
            return (TQuery)(object)this;
        }
        public TQuery InnerJoin(string column, string referenceTable, string referenceColumn)
        {
            _join.Add(new SqlJoin()
            {
                RefenreceTable = referenceTable,
            }.On(column, referenceColumn));
            return (TQuery)(object)this;
        }
        public TQuery LeftJoin(string column, string referenceTable, string referenceColumn)
        {
            _join.Add(new SqlJoin()
            {
                Join = "left",
                RefenreceTable = referenceTable,
            }.On(column, referenceColumn));
            return (TQuery)(object)this;
        }
        public TQuery RightJoin(string column, string referenceTable, string referenceColumn)
        {
            _join.Add(new SqlJoin()
            {
                Join = "right",
                RefenreceTable = referenceTable,
            }.On(column, referenceColumn));
            return (TQuery)(object)this;
        }
        public TQuery FullJoin(string column, string referenceTable, string referenceColumn)
        {
            _join.Add(new SqlJoin()
            {
                Join = "full outer",
                RefenreceTable = referenceTable,
            }.On(column, referenceColumn));
            return (TQuery)(object)this;
        }
        public TQuery CrossJoin(string column, string referenceTable, string referenceColumn)
        {
            _join.Add(new SqlJoin()
            {
                Join = "cross outer join",
                RefenreceTable = referenceTable,
            }.On(column, referenceColumn));
            return (TQuery)(object)this;
        }
        public TQuery CrossJoinInline<T>(IEnumerable<T> values, string column, string tableAs)
        {
            _join.Add(new SqlJoinInline<T>()
            {
                Join = "cross join",
                Column = column,
                TableAlias = tableAs,
                Values = values
            });
            return (TQuery)(object)this;
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
            return WhereOperator(new EqualOperator<T>() { Column = new SqlColumnFunction<TQuery>() { Query = New(), Function = function.Invoke(_functionFactory.New()) }, Value = value });
        }
        public TQuery Equal<T>(T value)
        {
            return WhereOperator(new EqualOperator<T>() { Column = _whereColumn, Value = value });
        }
        public TQuery Equal(string sql)
        {
            return WhereOperator(new EqualRawOperator() { Column = _whereColumn, RawSql = sql });
        }
        public TQuery WhereNotEqual<T>(string column, T value)
        {
            return WhereOperator(column, new NotEqualOperator<T>() { Column = new SqlColumn() { Column = column }, Value = value });
        }
        public TQuery NotEqual<T>(T value)
        {
            return WhereOperator(new NotEqualOperator<T>() { Column = _whereColumn, Value = value });
        }
        public TQuery NotEqual(string sql)
        {
            return WhereOperator(new NotEqualRawOperator() { Column = _whereColumn, RawSql = sql });
        }
        public TQuery WhereGreater<T>(string column, T value)
        {
            return WhereOperator(column, new GreaterThanOperator<T>() { Column = new SqlColumn() { Column = column }, Value = value });
        }
        public TQuery WhereGreater<T>(Func<IFunction, IFunction> function, T value)
        {
            return WhereOperator(new GreaterThanOperator<T>() { Column = new SqlColumnFunction<TQuery>() { Query = New(), Function = function.Invoke(_functionFactory.New()) }, Value = value });
        }
        public TQuery Greater<T>(T value)
        {
            return WhereOperator(new GreaterThanOperator<T>() { Column = _whereColumn, Value = value });
        }
        public TQuery Greater(string sql)
        {
            return WhereOperator(new GreaterThanRawOperator() { Column = _whereColumn, RawSql = sql });
        }
        public TQuery WhereNotGreater<T>(string column, T value)
        {
            return WhereOperator(column, new GreaterThanOperator<T>() { Column = new SqlColumn() { Column = column }, Value = value }.Not());
        }
        public TQuery NotGreater<T>(T value)
        {
            return WhereOperator(new GreaterThanOperator<T>() { Column = _whereColumn, Value = value }.Not());
        }
        public TQuery NotGreater(string sql)
        {
            return WhereOperator(new GreaterThanRawOperator() { Column = _whereColumn, RawSql = sql }.Not());
        }
        public TQuery WhereLess<T>(string column, T value)
        {
            return WhereOperator(column, new LessThanOperator<T>() { Column = new SqlColumn() { Column = column }, Value = value });
        }
        public TQuery WhereLess<T>(Func<IFunction, IFunction> function, T value)
        {
            return WhereOperator(new LessThanOperator<T>() { Column = new SqlColumnFunction<TQuery>() { Query = New(), Function = function.Invoke(_functionFactory.New()) }, Value = value });
        }
        public TQuery Less<T>(T value)
        {
            return WhereOperator(new LessThanOperator<T>() { Column = _whereColumn, Value = value });
        }
        public TQuery Less(string sql)
        {
            return WhereOperator(new LessThanRawOperator() { Column = _whereColumn, RawSql = sql });
        }
        public TQuery WhereNotLess<T>(string column, T value)
        {
            return WhereOperator(column, new LessThanOperator<T>() { Column = new SqlColumn() { Column = column }, Value = value }.Not());
        }
        public TQuery NotLess<T>(T value)
        {
            return WhereOperator(new LessThanOperator<T>() { Column = _whereColumn, Value = value }.Not());
        }
        public TQuery NotLess(string sql)
        {
            return WhereOperator(new LessThanRawOperator() { Column = _whereColumn, RawSql = sql }.Not());
        }
        public TQuery WhereGreaterOrEqual<T>(string column, T value)
        {
            return WhereOperator(column, new GreaterThanOrEqualOperator<T>() { Column = new SqlColumn() { Column = column }, Value = value });
        }
        public TQuery WhereGreaterOrEqual<T>(Func<IFunction, IFunction> function, T value)
        {
            return WhereOperator(new GreaterThanOrEqualOperator<T>() { Column = new SqlColumnFunction<TQuery>() { Query = New(), Function = function.Invoke(_functionFactory.New()) }, Value = value });
        }
        public TQuery GreaterOrEqual<T>(T value)
        {
            return WhereOperator(new GreaterThanOrEqualOperator<T>() { Column = _whereColumn, Value = value });
        }
        public TQuery GreaterOrEqual(string sql)
        {
            return WhereOperator(new GreaterThanOrEqualRawOperator() { Column = _whereColumn, RawSql = sql });
        }
        public TQuery WhereNotGreaterOrEqual<T>(string column, T value)
        {
            return WhereOperator(column, new GreaterThanOrEqualOperator<T>() { Column = new SqlColumn() { Column = column }, Value = value }.Not());
        }
        public TQuery NotGreaterOrEqual<T>(T value)
        {
            return WhereOperator(new GreaterThanOrEqualOperator<T>() { Column = _whereColumn, Value = value }.Not());
        }
        public TQuery NotGreaterOrEqual(string sql)
        {
            return WhereOperator(new GreaterThanOrEqualRawOperator() { Column = _whereColumn, RawSql = sql }.Not());
        }
        public TQuery WhereLessOrEqual<T>(string column, T value)
        {
            return WhereOperator(column, new LessThanOrEqualOperator<T>() { Column = new SqlColumn() { Column = column }, Value = value });
        }
        public TQuery WhereLessOrEqual<T>(Func<IFunction, IFunction> function, T value)
        {
            return WhereOperator(new LessThanOrEqualOperator<T>() { Column = new SqlColumnFunction<TQuery>() { Query = New(), Function = function.Invoke(_functionFactory.New()) }, Value = value });
        }
        public TQuery LessOrEqual<T>(T value)
        {
            return WhereOperator(new LessThanOrEqualOperator<T>() { Column = _whereColumn, Value = value });
        }
        public TQuery LessOrEqual(string sql)
        {
            return WhereOperator(new LessThanOrEqualRawOperator() { Column = _whereColumn, RawSql = sql });
        }
        public TQuery WhereNotLessOrEqual<T>(string column, T value)
        {
            return WhereOperator(column, new LessThanOrEqualOperator<T>() { Column = new SqlColumn() { Column = column }, Value = value }.Not());
        }
        public TQuery NotLessOrEqual<T>(T value)
        {
            return WhereOperator(new LessThanOrEqualOperator<T>() { Column = _whereColumn, Value = value }.Not());
        }
        public TQuery NotLessOrEqual(string sql)
        {
            return WhereOperator(new LessThanOrEqualRawOperator() { Column = _whereColumn, RawSql = sql }.Not());
        }
        public TQuery WhereBetween<T>(string column, T value, T secondValue)
        {
            return WhereOperator(column, new BetweenOperator() { Column = new SqlColumn() { Column = column }, Value = value, SecondValue = secondValue });
        }
        public TQuery Between<T>(T value, T secondValue)
        {
            return WhereOperator(new BetweenOperator() { Column = _whereColumn, Value = value, SecondValue = secondValue });
        }
        public TQuery Between(string sql)
        {
            return WhereOperator(new BetweenRawOperator() { Column = _whereColumn, RawSql = sql });
        }
        public TQuery WhereNotBetween<T>(string column, T value, T secondValue)
        {
            return WhereOperator(column, new BetweenOperator() { Column = new SqlColumn() { Column = column }, Value = value, SecondValue = secondValue }.Not());
        }
        public TQuery NotBetween<T>(T value, T secondValue)
        {
            return WhereOperator(new BetweenOperator() { Column = _whereColumn, Value = value, SecondValue = secondValue }.Not());
        }
        public TQuery NotBetween(string sql)
        {
            return WhereOperator(new BetweenRawOperator() { Column = _whereColumn, RawSql = sql }.Not());
        }
        public TQuery WhereIn<T>(string column, IEnumerable<T> value)
        {
            return WhereOperator(column, new InOperator() { Column = new SqlColumn() { Column = column }, Value = value });
        }
        public TQuery WhereIn<T>(Func<IFunction, IFunction> function, IEnumerable<T> value)
        {
            return WhereOperator(new InOperator() { Column = new SqlColumnFunction<TQuery>() { Query = New(), Function = function.Invoke(_functionFactory.New()) }, Value = value });
        }
        public TQuery In<T>(IEnumerable<T> value)
        {
            return WhereOperator(new InOperator() { Column = _whereColumn, Value = value });
        }
        public TQuery In(string sql)
        {
            return WhereOperator(new InRawOperator() { Column = _whereColumn, RawSql = sql });
        }
        public TQuery WhereNotIn<T>(string column, IEnumerable<T> value)
        {
            return WhereOperator(column, new InOperator() { Column = new SqlColumn() { Column = column }, Value = value }.Not());
        }
        public TQuery NotIn<T>(IEnumerable<T> value)
        {
            return WhereOperator(new InOperator() { Column = _whereColumn, Value = value }.Not());
        }
        public TQuery NotIn(string sql)
        {
            return WhereOperator(new InRawOperator() { Column = _whereColumn, RawSql = sql }.Not());
        }
        public TQuery WhereLike<T>(string column, T value)
        {
            return WhereOperator(column, new LikeOperator() { Column = new SqlColumn() { Column = column }, Value = value, LikeStarts = "%", LikeEnds = "%" });
        }
        public TQuery WhereLike<T>(Func<IFunction, IFunction> function, T value)
        {
            return WhereOperator(new LikeOperator() { Column = new SqlColumnFunction<TQuery>() { Query = New(), Function = function.Invoke(_functionFactory.New()) }, Value = value, LikeStarts = "%", LikeEnds = "%" });
        }
        public TQuery Like<T>(T value)
        {
            return WhereOperator(new LikeOperator() { Column = _whereColumn, Value = value, LikeStarts = "%", LikeEnds = "%" });
        }
        public TQuery Like(string sql)
        {
            return WhereOperator(new LikeRawOperator() { Column = _whereColumn, RawSql = sql, LikeStarts = "%", LikeEnds = "%" });
        }
        public TQuery WhereNotLike<T>(string column, T value)
        {
            return WhereOperator(column, new LikeOperator() { Column = new SqlColumn() { Column = column }, Value = value, LikeStarts = "%", LikeEnds = "%" }.Not());
        }
        public TQuery NotLike<T>(T value)
        {
            return WhereOperator(new LikeOperator() { Column = _whereColumn, Value = value, LikeStarts = "%", LikeEnds = "%" }.Not());
        }
        public TQuery NotLike(string sql)
        {
            return WhereOperator(new LikeRawOperator() { Column = _whereColumn, RawSql = sql, LikeStarts = "%", LikeEnds = "%" }.Not());
        }
        public TQuery WhereStarts<T>(string column, T value)
        {
            return WhereOperator(column, new LikeOperator() { Column = new SqlColumn() { Column = column }, Value = value, LikeStarts = "", LikeEnds = "%" });
        }
        public TQuery WhereStarts<T>(Func<IFunction, IFunction> function, T value)
        {
            return WhereOperator(new LikeOperator() { Column = new SqlColumnFunction<TQuery>() { Query = New(), Function = function.Invoke(_functionFactory.New()) }, Value = value, LikeStarts = "", LikeEnds = "%" });
        }
        public TQuery WhereNotStarts<T>(string column, T value)
        {
            return WhereOperator(column, new LikeOperator() { Column = new SqlColumn() { Column = column }, Value = value, LikeStarts = "", LikeEnds = "%" }.Not());
        }
        public TQuery Starts<T>(T value)
        {
            return WhereOperator(new LikeOperator() { Column = _whereColumn, Value = value, LikeStarts = "", LikeEnds = "%" });
        }
        public TQuery Starts(string sql)
        {
            return WhereOperator(new LikeRawOperator() { Column = _whereColumn, RawSql = sql, LikeStarts = "", LikeEnds = "%" });
        }
        public TQuery NotStarts<T>(T value)
        {
            return WhereOperator(new LikeOperator() { Column = _whereColumn, Value = value, LikeStarts = "", LikeEnds = "%" }.Not());
        }
        public TQuery NotStarts(string sql)
        {
            return WhereOperator(new LikeRawOperator() { Column = _whereColumn, RawSql = sql, LikeStarts = "", LikeEnds = "%" }.Not());
        }
        public TQuery WhereEnds<T>(string column, T value)
        {
            return WhereOperator(column, new LikeOperator() { Column = new SqlColumn() { Column = column }, Value = value, LikeStarts = "%", LikeEnds = "" });
        }
        public TQuery WhereEnds<T>(Func<IFunction, IFunction> function, T value)
        {
            return WhereOperator(new LikeOperator() { Column = new SqlColumnFunction<TQuery>() { Query = New(), Function = function.Invoke(_functionFactory.New()) }, Value = value, LikeStarts = "%", LikeEnds = "" });
        }
        public TQuery WhereNotEnds<T>(string column, T value)
        {
            return WhereOperator(column, new LikeOperator() { Column = new SqlColumn() { Column = column }, Value = value, LikeStarts = "%", LikeEnds = "" }.Not());
        }
        public TQuery Ends<T>(T value)
        {
            return WhereOperator(new LikeOperator() { Column = _whereColumn, Value = value, LikeStarts = "%", LikeEnds = "" });
        }
        public TQuery Ends(string sql)
        {
            return WhereOperator(new LikeRawOperator() { Column = _whereColumn, RawSql = sql, LikeStarts = "%", LikeEnds = "" });
        }
        public TQuery NotEnds<T>(T value)
        {
            return WhereOperator(new LikeOperator() { Column = _whereColumn, Value = value, LikeStarts = "%", LikeEnds = "" }.Not());
        }
        public TQuery NotEnds(string sql)
        {
            return WhereOperator(new LikeRawOperator() { Column = _whereColumn, RawSql = sql, LikeStarts = "%", LikeEnds = "" }.Not());
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
        public TQuery WhereRaw(string sql)
        {
            return WhereOperator(new RawOperator() { RawSql = sql }.Not());
        }
        public TQuery Raw(string rawSql)
        {
            _raw.Add(new SqlRaw()
            {
                RawSql = rawSql
            });
            return (TQuery)(object)this;
        }
        public TQuery AppendRaw(string rawSql)
        {
            _raw.Add(new SqlRaw()
            {
                RawSql = rawSql
            });
            return (TQuery)(object)this;
        }
        public TQuery AndOn(string column, string referenceColumn)
        {
            var join = _join.LastOrDefault();
            if (join == null) return (TQuery)(object)this;

            join.On(column, referenceColumn);
            return (TQuery)(object)this;
        }

        public TQuery AndOn<T>(string table, string column, T columnValue)
        {
            var join = _join.LastOrDefault();
            if (join == null) return (TQuery)(object)this;

            join.On(table, column, columnValue);
            return (TQuery)(object)this;
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

            And();
            return WhereOperator(clonedComparisonOperator);
        }
        public TQuery And<T>(T value, T? secondValue)
        {
            var comparison = _where.Where(e => e.Operator.IsOperatorType<AbstractComparisonOperator<T>>()).LastOrDefault();
            if (comparison == null) return (TQuery)(object)this;

            var clonedComparison = comparison.Clone();
            var clonedComparisonOperator = clonedComparison.Operator.And() as AbstractComparisonOperator<T>;
            clonedComparisonOperator.Value = value;

            And();
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

            Or();
            return WhereOperator(clonedComparisonOperator);
        }
        public TQuery Or<T>(T value, T? secondValue)
        {
            var comparison = _where.Where(e => e.Operator.IsOperatorType<AbstractComparisonOperator<T>>()).LastOrDefault();
            if (comparison == null) return (TQuery)(object)this;

            var clonedComparison = comparison.Clone();
            var clonedComparisonOperator = clonedComparison.Operator.Or() as AbstractComparisonOperator<T>;
            clonedComparisonOperator.Value = value;

            Or();
            return WhereOperator(clonedComparisonOperator);
        }


        public TQuery GroupBy()
        {
            _groupBy.Add(new SqlGroupBy()
            {
                OrderId = _groupBy.Count + 1
            });

            return (TQuery)(object)this;
        }
        public TQuery GroupBy(int orderId)
        {
            _groupBy.Add(new SqlGroupBy()
            {
                OrderId = orderId
            });

            return (TQuery)(object)this;
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
        public TQuery GroupByRaw(string sql)
        {
            _groupBy.Add(new SqlGroupByRaw()
            {
                RawSql = sql
            });

            return (TQuery)(object)this;
        }

        public TQuery Union(TQuery query)
        {
            _union.Add(new SqlUnion<TQuery>() { All = false, Query = query });
            return (TQuery)(object)this;
        }

        public TQuery UnionAll(TQuery query)
        {
            _union.Add(new SqlUnion<TQuery>() { All = true, Query = query });
            return (TQuery)(object)this;
        }

        public TQuery UnionAll(Func<TQuery, TQuery> query)
        {
            var newQuery = query(New());
            _union.Add(new SqlUnion<TQuery>() { All = true, Query = newQuery });
            return (TQuery)(object)this;
        }

        public TQuery OrderBy(int orderId, string? order = "asc")
        {
            _orderBy.Add(new SqlOrderBy()
            {
                OrderId = orderId,
                Order = order
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

            result.Sql = CompileTokens(result).Build();
            result.SqlParameters = CompileSqlParameters(result);
            result.NameParameters = CompileNameParameters(result);
            result = CompileSql(result);

            return result;
        }
        public virtual TQuery CompileFull(SqlQueryResult result)
        {
            return (TQuery)(object)this;
        }
    }
}
