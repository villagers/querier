using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class QueryTableAttribute : BaseAttribute, ITableAttribute
    {
        public string? Table { get; set; }
        public QueryTableAttribute(string table)
        {
            Table = table;
        }
    }
}
