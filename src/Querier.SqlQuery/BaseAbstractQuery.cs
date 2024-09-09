using Querier.SqlQuery.Extensions;
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
        protected readonly List<ISqlSelect> _select;
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
            _select = new List<ISqlSelect>();
            _where = new List<SqlWhere>();
            _groupBy = new List<SqlGroupBy>();
            _orderBy = new List<SqlOrderBy>();

            SqlParameters = new Dictionary<string, object>();
            NameParameters = new Dictionary<string, string>();
        }

        public virtual SqlQueryResult CompileSql(SqlQueryResult result)
        {
            result.CompiledSql = new string(result.Sql);
            foreach (var param in result.NameParameters)
            {
                if (param.Value == "*")
                {
                    result.CompiledSql = result.CompiledSql.ReplaceExact(param.Key, $"{param.Value}");
                    continue;
                }
                result.CompiledSql = result.CompiledSql.ReplaceExact(param.Key, $"{NameParameterOpening}{param.Value}{NameParameterClosing}");
            }

            return result;
        }
        public virtual SqlTokenizer CompileTokens(SqlQueryResult result)
        {
            return CreateSelect()
                .Merge(CreateTable())
                .Merge(CreateWhere())
                .Merge(CreateGroupBy())
                .Merge(CreateOrderBy())
                .SqlTokenizer;
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
                    result.Sql = result.Sql.ReplaceExact(e.Key, $"{NameParameterPlaceholder}{i}");
                    return new KeyValuePair<string, string>($"{NameParameterPlaceholder}{i}", e.Value);
                }).ToDictionary();
        }
        public virtual SqlQueryResult CreateTable()
        {
            var result = new SqlQueryResult();

            var tableTz = new SqlTokenizer().AddToken("from");

            var tableCompiled = _table.Compile();
            result = result.Merge(tableCompiled);
            tableTz.AddToken(tableCompiled.Sql);

            result.NameParameters.CopyTo(NameParameters);
            result.SqlParameters.CopyTo(SqlParameters);

            result.Sql = tableTz.Build(" ");
            return result.Enumerate();
        }
        public virtual SqlQueryResult CreateSelect()
        {
            var result = new SqlQueryResult();

            var selectTz = new SqlTokenizer().AddToken("select");
            if (_select.Count <= 0)
            {
                result.Sql = selectTz.AddToken("*").Build(" ");
                result.SqlTokenizer = selectTz;
                return result.Enumerate();
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
                    result = result.Merge(selectCompiled);
                    tz.AddToken(selectCompiled.Sql);
                }

                return tz;
            }, ", ");

            result.NameParameters.CopyTo(NameParameters);
            result.SqlParameters.CopyTo(SqlParameters);

            result.SqlTokenizer = selectTz;
            result.Sql = selectTz.Build(" ");
            return result.Enumerate();
        }
        public virtual SqlQueryResult CreateWhere()
        {
            var result = new SqlQueryResult();

            if (_where.Count <= 0) return result;

            var whereTz = new SqlTokenizer().AddToken("where");

            foreach (var where in _where)
            {
                var whereCompiled = where.Compile();
                result = result.Merge(whereCompiled);
                whereTz.AddToken(whereCompiled.Sql);
            }

            result.NameParameters.CopyTo(NameParameters);
            result.SqlParameters.CopyTo(SqlParameters);

            result.Sql = whereTz.Build();
            return result.Enumerate();
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
                    result = result.Merge(groupByCompiled);
                    e.AddToken(groupByCompiled.Sql);
                }
                return e;
            }, ", ");

            result.NameParameters.CopyTo(NameParameters);
            result.SqlParameters.CopyTo(SqlParameters);

            result.Sql = groupTz.Build();
            return result.Enumerate();
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
                    var orderByCompiled = order.Compile();
                    result = result.Merge(orderByCompiled);
                    e.AddToken(orderByCompiled.Sql);
                }
                return e;
            }, ", ");

            result.NameParameters.CopyTo(NameParameters);
            result.SqlParameters.CopyTo(SqlParameters);

            result.Sql = orderTz.Build();
            return result.Enumerate();
        }
    }
}
