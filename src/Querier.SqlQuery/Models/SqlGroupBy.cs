using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Models
{
    public class SqlGroupBy : ISqlQueryCompile<SqlQueryResult>
    {
        public virtual required string Column { get; set; }

        public virtual SqlQueryResult Compile()
        {
            var result = new SqlQueryResult();
            var selectTz = new SqlTokenizer();

            selectTz.AddToken("@column");
            result.NameParameters.Add("@column", Column);

            result.Sql = selectTz.Build(" ");
            result.NameParameters = result.NameParameters.Select((e, i) =>
            {
                result.Sql = result.Sql.Replace(e.Key, $"@name{i}");
                return new KeyValuePair<string, string>($"@name{i}", e.Value);
            }).ToDictionary();

            return result;
        }
    }
}
