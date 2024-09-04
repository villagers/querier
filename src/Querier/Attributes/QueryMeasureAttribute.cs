using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class QueryMeasureAttribute : BaseAttribute, IKeyAttribute, IDisplayAttribute
    {
        public string Key { get; set; }
        public string DisplayName { get; set; }

        public QueryMeasureAttribute() { }
        public QueryMeasureAttribute(string key)
        {
            Key = key;
        }
        public QueryMeasureAttribute(string key, string displayName)
        {
            Key = key;
            DisplayName = displayName;
        }
    }
}
