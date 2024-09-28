using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class QueryTimeDimensionAttribute : BaseAttribute, IKeyAttribute, IAliasAttribute, IOrderAttribute, IDescriptionAttribute, IColumnAttribute, ISqlAttribute, IGranularityAttribute
    {
        public string? Sql { get; set; }
        public string? Key { get; set; }
        public string? Alias { get; set; }
        public string? Order { get; set; }
        public string? Column { get; set; }
        public string? Description { get; set; }
        public string? Granularity { get; set; }
        public QueryTimeDimensionAttribute() { }
        public QueryTimeDimensionAttribute(string key)
        {
            Key = key;
        }
    }
}
