using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class QueryAggregationAttribute : BaseAttribute, IAggregationAttribute
    {
        public string? Aggregation {  get; set; }
        public QueryAggregationAttribute(string aggregation)
        {
            Aggregation = aggregation;
        }
    }
}
