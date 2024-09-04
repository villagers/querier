using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class QueryDimensionAttribute : BaseAttribute, IKeyAttribute, IDisplayAttribute
    {
        public string Key { get; set; }
        public string DisplayName { get; set; }

        public QueryDimensionAttribute() { }
        public QueryDimensionAttribute(string key)
        {
            Key = key;
        }
        public QueryDimensionAttribute(string key, string displayName)
        {
            Key = key;
            DisplayName = displayName;
        }
    }
}
