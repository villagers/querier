using DuckDB.NET.Data;
using Querier.Attributes;
using Querier.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Schema
{
    public class QuerySchema : ISqlDescriptor, IKeyDescriptor, IAliasDescriptor, ITableDescriptor, IDescriptionDescriptor
    {
        public string? Sql { get; set; }
        public string? Key { get; set; }
        public string? Alias { get; set; }
        public string? Table { get; set; }
        public required string DbFile { get; set; }
        public string? Description { get; set; }
        

        public string? RefreshSql { get; set; }
        public string RefreshInterval { get; set; } = "* * * * *";

        public required Type Type { get; set; }
        public bool RunOnceAtStart = true;

        public SchemaQueryCommand? SchemaCommand { get; set; }
        public string SchemaCommandDuckDbTable { get; set; }
        public string SchemaCommandDuckDbConfigTable { get; set; }

        public readonly HashSet<QueryMeasureSchema> Measures;
        public readonly HashSet<QueryDimensionSchema> Dimensions;
        public readonly HashSet<QueryTimeDimensionSchema> TimeDimensions;

        public QuerySchema()
        {
            Measures = new HashSet<QueryMeasureSchema>();
            Dimensions = new HashSet<QueryDimensionSchema>();
            TimeDimensions = new HashSet<QueryTimeDimensionSchema>();

            SchemaCommandDuckDbTable = "DROP TABLE IF EXISTS {0}; CREATE TABLE {0} ({1})";
            SchemaCommandDuckDbConfigTable = "DROP TABLE IF EXISTS config; CREATE TABLE config (query VARCHAR, refresh_key VARCHAR)";
        }
    }
}
