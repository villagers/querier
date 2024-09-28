using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class QueryColumnAttribute : BaseAttribute, IColumnAttribute
    {
        public string? Column { get; set; }
        public QueryColumnAttribute() { }
        public QueryColumnAttribute(string column)
        {
            Column = column;
        }
    }
}
