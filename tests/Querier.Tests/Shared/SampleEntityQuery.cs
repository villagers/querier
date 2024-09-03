using Querier.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Tests.Shared
{
    [QueryKey("SEQ")]
    [Query(Key = "SEQ2")]
    internal class SampleEntityQuery
    {
        [QueryKey("IID")]
        [QueryDisplay("Invoice ID")]
        [QueryDimension(Key = "ID")]
        public int Id { get; set; }

        [QueryMeasure]
        [QueryDimension(Key = "NAME")]
        public string Name { get; set; }

        [QueryMeasure]
        public string Description { get; set; }
        public long Total { get; set; }
        public DateTime Date { get; set; }
    }
}
