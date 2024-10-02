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
