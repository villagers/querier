using Querier.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Schema
{
    public class QueryDimensionSchema : ISqlDescriptor, IKeyDescriptor, IAliasDescriptor, IOrderDescriptor, IColumnDescriptor, IDescriptionDescriptor, IGranularityDescriptor
    {
        public string? Sql { get; set; }
        public string? Key { get; set; }
        public string? Alias { get; set; }
        public string? Order { get; set; }
        public string? Column { get; set; }
        public string? Description { get; set; }
        public string? Granularity { get; set; }

        public required Type Type { get; set; }
    }
}
