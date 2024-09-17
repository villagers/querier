using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Operators;
using Querier.SqlQuery.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Models
{
    public class SqlCaseWhen : ISqlQueryCompile<SqlQueryResult>
    {
        public required AbstractOperator Operator { get; set; }
        public required object Value { get; set; }

        public SqlQueryResult Compile(ISqlTable table)
        {
            var selectTz = new SqlTokenizer();

            var compiledOperator = Operator.Compile(table);
            compiledOperator.SqlParameters.Add("@caseValue", Value);

            selectTz
                .AddToken("when")
                .AddToken(compiledOperator.Sql)
                .AddToken("then")
                .AddToken("@caseValue");

            var result = new SqlQueryResult()
            {
                Sql = selectTz.Build(),
                NameParameters = compiledOperator.NameParameters,
                SqlParameters = compiledOperator?.SqlParameters ?? []
            };

            return result.Enumerate();
        }
    }
}
