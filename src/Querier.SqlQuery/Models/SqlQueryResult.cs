using Querier.SqlQuery.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Models
{
    public class SqlQueryResult
    {
        public string Sql { get; set; } = string.Empty;
        public string CompiledSql { get; set; } = string.Empty;
        public SqlTokenizer SqlTokenizer { get; set; }
        public Dictionary<string, object> SqlParameters { get; set; }
        public Dictionary<string, string> NameParameters { get; set; }

        public SqlQueryResult()
        {
            SqlTokenizer = new SqlTokenizer();
            SqlParameters = new Dictionary<string, object>();
            NameParameters = new Dictionary<string, string>();
        }
    }
}
