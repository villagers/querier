using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Models;
using Querier.SqlQuery.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Operators
{
    public class IsNullOperator : AbstractOperator
    {
        public override SqlQueryResult Compile(ISqlTable table)
        {
            var column = Column.Compile(table);

            var sqlTz = new SqlTokenizer()
                .AddToken(AndOrOperator)
                .AddToken(column.Sql)
                .AddToken("is")
                .AddToken(NotOperator)
                .AddToken("null")
                .Build();

            var result = new SqlQueryResult()
            {
                Sql = sqlTz,
                NameParameters = column.NameParameters,
            };
            return result.Enumerate();
        }
    }
}
