using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class QueryAttribute : BaseAttribute, IKeyAttribute, IDisplayAttribute
    {
        public string Key { get; set; }
        public string DisplayName { get; set; }

        public QueryAttribute() { }
        public QueryAttribute(string key, string displayName)
        {
            Key = key;
            DisplayName = displayName;
        }
    }
}
