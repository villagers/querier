using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class QuerySqlAttribute : BaseAttribute, ISqlAttribute
    {
        public string? Sql { get; set; }
        public QuerySqlAttribute(string sql)
        {
            Sql = sql;
        }
    }
}
