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
    public class SqlSelectRaw : ISqlSelect
    {
        public required string RawSql { get; set; }
        public required string RawSqlAs { get; set; }
        public virtual SqlQueryResult Compile(ISqlTable table)
        {
            var result = new SqlQueryResult();
            var tokenizer = new SqlTokenizer($"({RawSql})").AddToken("as").AddToken("@as");
            return new SqlQueryResult()
            {
                Sql = tokenizer.Build(),
                NameParameters = new Dictionary<string, string>() { { "@as", RawSqlAs } }
            };
        }
    }
}
