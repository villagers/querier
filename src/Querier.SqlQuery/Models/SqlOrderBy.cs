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

        public virtual SqlQueryResult Compile()
        {
            var result = new SqlQueryResult();
            var selectTz = new SqlTokenizer();

            selectTz.AddToken("@column").AddToken(Order);
            result.NameParameters.Add("@column", Column);

            result.Sql = selectTz.Build();
            return result.Enumerate();
        }
    }
}
