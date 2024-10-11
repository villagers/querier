using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Models
{
    public class SqlJoinTable : ISqlJoin
    {
        public string Join { get; set; } = "inner";
        public required string Table { get; set; }
        public SqlQueryResult Compile(ISqlTable table)
        {
            var result = new SqlQueryResult();
            result.NameParameters.Add("@table", Table);

            var tz = new SqlTokenizer()
                .AddToken($"{Join} join")
                .AddToken("@table");

            result.Sql = tz.Build();
            return result.Enumerate();
        }

        public ISqlJoin On(string column, string referenceColumn)
        {
            throw new NotImplementedException();
        }

        public ISqlJoin On<T>(string table, string column, T columnValue)
        {
            throw new NotImplementedException();
        }
    }
}
