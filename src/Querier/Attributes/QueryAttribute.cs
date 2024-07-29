using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class QueryAttribute : Attribute
    {
        public string Key;

        public QueryAttribute(string key)
        {
            this.Key = key;
        }
    }
}
