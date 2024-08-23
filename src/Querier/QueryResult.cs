using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier
{
    public class QueryResult
    {
        public IEnumerable<Dictionary<string, object>>? Data { get; set; }
        //public List<QueryFilter> Filters { get; set; } = new List<QueryFilter>();
        public List<QueryProperty> Measures { get; set; } = new List<QueryProperty>();
        public List<QueryProperty> Dimensions { get; set; } = new List<QueryProperty>();
        public List<QueryProperty> TimeDimensions { get; set; } = new List<QueryProperty>();
    }
}
