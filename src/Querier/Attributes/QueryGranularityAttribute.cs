using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class QueryGranularityAttribute : BaseAttribute, IGranularityAttribute
    {
        public string? Granularity { get; set; }
        public QueryGranularityAttribute(string? granularity)
        {
            Granularity = granularity;
        }
    }
}
