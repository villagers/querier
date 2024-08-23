using Querier.SqlQuery.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Models
{
    public class SqlFunctionResult
    {
        public string Sql { get; set; } = string.Empty;
        public Dictionary<string, string> NameParameters { get; set; }

        public SqlFunctionResult()
        {
            NameParameters = new Dictionary<string, string>();
        }
    }
}
