using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class QueryAliasAttribute : Attribute, IAliasAttribute
    {
        public string? Alias { get; set; }

        public QueryAliasAttribute() { }
        public QueryAliasAttribute(string alias)
        {
            Alias = alias;
        }
    }
}
