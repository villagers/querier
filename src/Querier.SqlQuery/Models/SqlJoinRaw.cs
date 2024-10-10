using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Tokenizers;

namespace Querier.SqlQuery.Models
{
    public class SqlJoinRaw : ISqlJoin
    {
        public required string RawSql { get; set; }
        public required string RefenreceTable { get; set; }
        public List<ISqlJoinOn> JoinOn { get; set; } = new List<ISqlJoinOn>();

        public SqlQueryResult Compile(ISqlTable table)
        {
            var result = new SqlQueryResult();
            result.Sql = new SqlTokenizer().AddToken($"{RawSql}").Build();

            result.NameParameters.Add("@refTable", RefenreceTable);
            result.NameParameters.Add("@table", table.TableOrAlias);

            var tz = new SqlTokenizer().AddToken(RawSql);

            for (var i = 0; i < JoinOn.Count; i++)
            {
                tz.AddToken("and");
                var join = JoinOn[i];
                var compiled = join.Compile(table);
                result = result.Merge(compiled);
                tz.AddToken(compiled.Sql);
            }

            result.Sql = tz.Build();
            return result.Enumerate();
        }

        public ISqlJoin On(string column, string referenceColumn)
        {
            JoinOn.Add(new SqlJoinOn()
            {
                Column = column,
                ReferenceColumn = referenceColumn,
                ReferenceTable = RefenreceTable
            });
            return this;
        }
        public ISqlJoin On<T>(string table, string column, T columnValue)
        {
            JoinOn.Add(new SqlJoinOnValue()
            {
                Table = table,
                Column = column,
                Value = columnValue
            });
            return this;
        }
    }
}
