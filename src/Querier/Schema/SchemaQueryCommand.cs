using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Schema
{
    public class SchemaQueryCommand
    {
        public required string Sql { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
        public SchemaQueryCommand()
        {
            Parameters = new Dictionary<string, object>();
        }
    }
}
