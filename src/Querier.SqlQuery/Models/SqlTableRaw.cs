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
    public class SqlTableRaw : ISqlTable
    {
        public required string RawSql { get; set; }
        public string TableOrAlias => "t1";

        public virtual SqlQueryResult Compile(ISqlTable table)
        {
            var result = new SqlQueryResult();

            var selectTz = new SqlTokenizer()
                .AddToken($"({RawSql})")
                .AddToken("as")
                .AddToken("@table");


            result.NameParameters.Add("@table", TableOrAlias);

            result.Sql = selectTz.Build(" ");
            return result.Enumerate();
        }
    }
}
