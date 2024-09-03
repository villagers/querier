using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class QueryDisplayAttribute : Attribute, IDisplayAttribute
    {
        public string DisplayName { get; set; }

        public QueryDisplayAttribute() { }
        public QueryDisplayAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }
}
