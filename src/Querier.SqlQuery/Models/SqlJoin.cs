using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Tokenizers;

namespace Querier.SqlQuery.Models
{
    public class SqlJoin : ISqlJoin
    {
        public required string RefenreceTable { get; set; }
        public string Join { get; set; } = "inner";
        public List<ISqlJoinOn> JoinOn { get; set; } = new List<ISqlJoinOn>();

        public SqlQueryResult Compile(ISqlTable table)
        {
            var result = new SqlQueryResult();
            result.NameParameters.Add("@refTable", RefenreceTable);
            result.NameParameters.Add("@table", table.TableOrAlias);

            var tz = new SqlTokenizer()
                .AddToken($"{Join} join")
                .AddToken("@refTable")
                .AddToken("on");
            

            for (var i = 0; i < JoinOn.Count; i++)
            {
                if (i > 0)
                {
                    tz.AddToken("and");
                }
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
