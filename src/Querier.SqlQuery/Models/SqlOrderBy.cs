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
    public class SqlOrderBy : ISqlQueryCompile<SqlQueryResult>
    {
        public required string Column { get; set; }
        public required string Order { get; set; }

        public virtual SqlQueryResult Compile(ISqlTable table)
        {
            var result = new SqlQueryResult();
            var selectTz = new SqlTokenizer();

            result.NameParameters.Add("@table", table.TableOrAlias);
            result.NameParameters.Add("@column", Column);

            selectTz.AddToken(e => e.AddToken("@table").AddToken(".").AddToken("@column"), "").AddToken(Order);
            

            result.Sql = selectTz.Build();
            return result.Enumerate();
        }
    }
}
