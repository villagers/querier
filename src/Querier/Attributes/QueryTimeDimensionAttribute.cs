using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class QueryTimeDimensionAttribute : BaseAttribute, IKeyAttribute, IDisplayAttribute
    {
        public string Key { get; set; }
        public string DisplayName { get; set; }

        public QueryTimeDimensionAttribute() { }
        public QueryTimeDimensionAttribute(string key)
        {
            Key = key;
        }
        public QueryTimeDimensionAttribute(string key, string displayName)
        {
            Key = key;
            DisplayName = displayName;
        }
    }
}
