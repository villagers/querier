using Querier.SqlQuery.Functions;
using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Models;
using Querier.SqlQuery.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery
{
    public class BaseAbstractQuery<TQuery> where TQuery : IBaseQuery<TQuery>
    {

        protected readonly IFunction _functionFactory;

        public SqlTable<TQuery> _table;
        protected readonly List<SqlSelect> _select;
        protected readonly List<SqlWhere> _where;
        protected readonly List<SqlGroupBy> _groupBy;
        protected readonly List<SqlOrderBy> _orderBy;



        protected bool _whereAnd = false;
        protected bool _whereOr = false;
        protected SqlColumn _whereColumn;

        protected bool _distinct = false;
        protected int? _limit;
        protected virtual string NameParameterOpening { get; set; } = "";
        protected virtual string NameParameterClosing { get; set; } = "";
        protected virtual string NameParameterPlaceholder { get; set; } = "@n";
        protected virtual string SqlParameterPlaceholder { get; set; } = "@p";

        protected Dictionary<string, object> SqlParameters;
        protected Dictionary<string, string> NameParameters;

        public BaseAbstractQuery(IFunction functionFactory)
        {
            _functionFactory = functionFactory;

            _table = new SqlTable<TQuery>();
            _select = new List<SqlSelect>();
            _where = new List<SqlWhere>();
            _groupBy = new List<SqlGroupBy>();
            _orderBy = new List<SqlOrderBy>();

            SqlParameters = new Dictionary<string, object>();
            NameParameters = new Dictionary<string, string>();
        }

        public virtual SqlQueryResult CompileSql(SqlQueryResult result)
        {
            foreach (var param in result.NameParameters)
            {
                result.CompiledSql = result.CompiledSql.Replace(param.Key, $"{NameParameterOpening}{param.Value}{NameParameterClosing}");
            }

            return result;
        }
        public virtual SqlTokenizer CompileTokens(SqlQueryResult result)
        {
            var queryTz = new SqlTokenizer();

            var selectQuery = CreateSelect();
            var tableQuery = CreateTable();
            var whereQuery = CreateWhere();
            var groupByQuery = CreateGroupBy();
            var orderByQuery = CreateOrderBy();

            queryTz.AddToken(selectQuery.Sql);
            queryTz.AddToken(tableQuery.Sql);
            queryTz.AddToken(whereQuery.Sql);
            queryTz.AddToken(groupByQuery.Sql);
            queryTz.AddToken(orderByQuery.Sql);

            return queryTz;
        }
        public virtual Dictionary<string, object> CompileSqlParameters(SqlQueryResult result)
        {
            return SqlParameters.Select((e, i) => new KeyValuePair<string, object>($"{SqlParameterPlaceholder}{i}", e.Value)).ToDictionary();
        }
        public virtual Dictionary<string, string> CompileNameParameters(SqlQueryResult result)
        {
            return NameParameters
                .Select((e, i) =>
                {
                    result.Sql = result.Sql.Replace(e.Key, $"{NameParameterPlaceholder}{i}");
                    return new KeyValuePair<string, string>($"{NameParameterPlaceholder}{i}", e.Value);
                }).ToDictionary();
        }
        public virtual SqlQueryResult CreateTable()
        {
            var result = new SqlQueryResult();

            var tableTz = new SqlTokenizer().AddToken("from");

            var tableCompiled = _table.Compile();
            var tableCompiledSql = tableCompiled.Sql;

            foreach (var parameter in tableCompiled.NameParameters)
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

                    foreach (var parameter in selectCompiled.NameParameters)
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
                var whereCompiled = where.Compile();
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

                foreach (var parameter in whereCompiled.NameParameters)
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
                    var groupByCompiled = group.Compile();
                    var groupByCompiledSql = groupByCompiled.Sql;

                    foreach (var parameter in groupByCompiled.NameParameters)
                    {
                        var nameCount = NameParameters.Count;
                        var newPlaceholder = $"{NameParameterPlaceholder}{nameCount}";

                        groupByCompiledSql = groupByCompiledSql.Replace(parameter.Key, newPlaceholder);

                        NameParameters.Add(newPlaceholder, parameter.Value);
                        result.NameParameters.Add(newPlaceholder, parameter.Value);
                    }

                    e.AddToken(groupByCompiledSql);
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
