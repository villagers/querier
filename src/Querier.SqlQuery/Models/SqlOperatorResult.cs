using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Models
{
    public class SqlOperatorResult
    {
        public string? Sql { get; set; }
        public Dictionary<string, object> SqlParameters { get; set; }
        public Dictionary<string, string> NameParameters { get; set; }
        public SqlOperatorResult()
        {
            SqlParameters = new Dictionary<string, object>();
            NameParameters = new Dictionary<string, string>();
        }
    }
}
