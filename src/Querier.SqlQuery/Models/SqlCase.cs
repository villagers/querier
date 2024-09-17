using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Functions;
using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Operators;
using Querier.SqlQuery.Tokenizers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Models
{
    public class SqlCase : ISqlSelect
    {
        private readonly List<SqlCaseWhen> _sqlWhen;
        public string? CaseAs { get; set; }
        public required object ElseValue { get; set; }

        public SqlCase()
        {
            _sqlWhen = new List<SqlCaseWhen>();
        }

        public SqlCase AddCaseWhen(SqlCaseWhen caseWhen)
        {
            _sqlWhen.Add(caseWhen);
            return this;
        }

        public SqlQueryResult Compile(ISqlTable table)
        {
            var selectTz = new SqlTokenizer();
            var result = new SqlQueryResult();

            selectTz.AddToken("case");
            foreach (var caseWhen in _sqlWhen)
            {
                var compiledWhen = caseWhen.Compile(table);
                selectTz.AddToken(compiledWhen.Sql);

                result.SqlParameters = result.SqlParameters.Merge(compiledWhen.SqlParameters, "@value");
                result.NameParameters = result.NameParameters.Merge(compiledWhen.NameParameters);

                result.Enumerate();
            }

            result.SqlParameters.Add("@elseValue", ElseValue);
            selectTz.AddToken("else").AddToken("@elseValue").AddToken("end");

            if (!string.IsNullOrEmpty(CaseAs))
            {
                result.NameParameters.Add("@as", CaseAs);
                selectTz.AddToken("as").AddToken("@as");
            }

            result.Sql = selectTz.Build();
            return result.Enumerate();
        }
    }
}
