using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class QueryMeasureAttribute : Attribute
    {
        public string? Key { get; set; }
        public string? DisplayName { get; set; }
        public string? Format { get; set; } = "number";
    }
}
