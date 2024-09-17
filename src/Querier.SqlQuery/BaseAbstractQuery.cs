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

        public ISqlTable _table;
        protected readonly List<ISqlSelect> _select;
        protected readonly List<SqlJoin> _join;
        protected readonly List<SqlWhere> _where;
        protected readonly List<ISqlGroupBy> _groupBy;
        protected readonly List<SqlUnion<TQuery>> _union;
        protected readonly List<SqlOrderBy> _orderBy;

        protected readonly Dictionary<string, string> _alias;


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

            _select = new List<ISqlSelect>();
            _join = new List<SqlJoin>();
            _where = new List<SqlWhere>();
            _groupBy = new List<ISqlGroupBy>();
            _union = new List<SqlUnion<TQuery>>();
            _orderBy = new List<SqlOrderBy>();
            _alias = new Dictionary<string, string>();

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
                .Merge(CreateJoin())
                .Merge(CreateWhere())
                .Merge(CreateGroupBy())
                .Merge(CreateUnion())
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
                    var selectCompiled = select.Compile(_table);
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

        public virtual SqlQueryResult CreateJoin()
        {
            var result = new SqlQueryResult();

            if (_join.Count <= 0) return result;

            var tz = new SqlTokenizer();
            foreach (var join in _join)
            {
                var compiled = join.Compile(_table);
                result = result.Merge(compiled);
                tz.AddToken(compiled.Sql);
            }

            result.NameParameters.CopyTo(NameParameters);
            result.SqlParameters.CopyTo(SqlParameters);

            result.SqlTokenizer = tz;
            result.Sql = tz.Build();
            return result.Enumerate();
        }
        public virtual SqlQueryResult CreateTable()
        {
            var result = new SqlQueryResult();

            var tableTz = new SqlTokenizer().AddToken("from");

            var tableCompiled = _table.Compile(_table);
            result = result.Merge(tableCompiled);
            tableTz.AddToken(tableCompiled.Sql);

            result.NameParameters.CopyTo(NameParameters);
            result.SqlParameters.CopyTo(SqlParameters);

            result.SqlTokenizer = tableTz;
            result.Sql = tableTz.Build();
            return result.Enumerate();
        }
        public virtual SqlQueryResult CreateWhere()
        {
            var result = new SqlQueryResult();

            if (_where.Count <= 0) return result;

            var whereTz = new SqlTokenizer().AddToken("where");

            foreach (var where in _where)
            {
                var whereCompiled = where.Compile(_table);
                result = result.Merge(whereCompiled);
                whereTz.AddToken(whereCompiled.Sql);
            }

            result.NameParameters.CopyTo(NameParameters);
            result.SqlParameters.CopyTo(SqlParameters);

            result.SqlTokenizer = whereTz;
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
                    var groupByCompiled = group.Compile(_table);
                    result = result.Merge(groupByCompiled);
                    e.AddToken(groupByCompiled.Sql);
                }
                return e;
            }, ", ");

            result.NameParameters.CopyTo(NameParameters);
            result.SqlParameters.CopyTo(SqlParameters);

            result.SqlTokenizer = groupTz;
            result.Sql = groupTz.Build();
            return result.Enumerate();
        }
        public virtual SqlQueryResult CreateUnion()
        {
            var result = new SqlQueryResult();

            if (_union.Count <= 0) return result;

            var _unionTz = new SqlTokenizer();
            foreach (var union in _union)
            {
                var unionCompiled = union.Compile(_table);
                result = result.Merge(unionCompiled);
                _unionTz.AddToken(unionCompiled.Sql);
            }

            result.NameParameters.CopyTo(NameParameters);
            result.SqlParameters.CopyTo(SqlParameters);

            result.SqlTokenizer = _unionTz;
            result.Sql = _unionTz.Build();
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
                    var orderByCompiled = order.Compile(_table);
                    result = result.Merge(orderByCompiled);
                    e.AddToken(orderByCompiled.Sql);
                }
                return e;
            }, ", ");

            result.NameParameters.CopyTo(NameParameters);
            result.SqlParameters.CopyTo(SqlParameters);

            result.SqlTokenizer = orderTz;
            result.Sql = orderTz.Build();
            return result.Enumerate();
        }
    }
}
