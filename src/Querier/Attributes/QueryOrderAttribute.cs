using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class QueryOrderAttribute : BaseAttribute, IOrderAttribute
    {
        public string? Order { get; set; } = "asc";
        public QueryOrderAttribute(string order)
        {
            Order = order;
        }
    }
}
