using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Schema
{
    public class SchemaStore
    {
        public readonly HashSet<QuerySchema> Schemas;
        public string? LocalStoragePath { get; set; }

        public SchemaStore()
        {
            Schemas = new HashSet<QuerySchema>();
        }

        public string DataSource(QuerySchema schema)
        {
            var path = Path.Combine(LocalStoragePath ?? "", schema.DbFile);
            return $"DataSource={path}";
        }
    }
}
