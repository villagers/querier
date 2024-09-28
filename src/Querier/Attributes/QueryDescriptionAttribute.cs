using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class QueryDescriptionAttribute : BaseAttribute, IDescriptionAttribute
    {
        public string? Description { get; set; }
        public QueryDescriptionAttribute(string description) 
        {
            Description = description;
        }
    }
}
