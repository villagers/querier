namespace Querier.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class QueryAttribute : BaseAttribute, IKeyAttribute, IAliasAttribute, IDescriptionAttribute, ITableAttribute, ISqlAttribute
    {
        public string? Sql { get; set; }
        public string? Key { get; set; }
        public string? Alias { get; set; }
        public string? Table {  get; set; }
        public string? Description { get; set; }

        public string? RefreshSql { get; set; }
        public string? RefreshInterval { get; set; }

        public bool WarmUp { get; set; } = true;

        public QueryAttribute() { }
        public QueryAttribute(string key)
        {
            Key = key;
        }
    }
}
