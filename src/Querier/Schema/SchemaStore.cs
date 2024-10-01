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
        public bool Initialized => Schemas.All(e => e.Initialized);

        public SchemaStore()
        {
            Schemas = new HashSet<QuerySchema>();
        }

        public string DataSource(QuerySchema schema)
        {
            var dbFile = schema.DbFile;
            var extension = Path.GetExtension(dbFile);
            if (string.IsNullOrWhiteSpace(extension))
            {
                dbFile += ".db";
            }
            var path = Path.Combine(LocalStoragePath, dbFile);
            return $"DataSource={path}";
        }
    }
}
