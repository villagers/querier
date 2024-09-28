using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class QueryMeasureAttribute : BaseAttribute, IKeyAttribute, IAliasAttribute, IOrderAttribute, IDescriptionAttribute, IColumnAttribute, ISqlAttribute, IAggregationAttribute
    {
        public string? Sql { get; set; }
        public string? Key { get; set; }
        public string? Alias { get; set; }
        public string? Order { get; set; }
        public string? Column { get; set; }
        public string? Description { get; set; }
        public string? Aggregation { get; set; }

        public QueryMeasureAttribute() { }
        public QueryMeasureAttribute(string key)
        {
            Key = key;
        }
    }
}
