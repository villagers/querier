using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Models;
using Querier.SqlQuery.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Operators
{
    public class GreaterThanOrEqualOperator<T> : AbstractComparisonOperator<T>
    {
        public override SqlQueryResult Compile()
        {
            var column = Column.Compile();

            var sqlTz = new SqlTokenizer()
                .AddToken(AndOrOperator)
                .AddToken(NotOperator)
                .AddToken(column.Sql)
                .AddToken(">=")
                .AddToken("@value")
                .Build();

            var result = new SqlQueryResult()
            {
                Sql = sqlTz,
                NameParameters = column.NameParameters,
                SqlParameters = new Dictionary<string, object>()
                {
                    { "@value", Value }
                }
            };

            return result.Enumerate();
        }
    }
}
