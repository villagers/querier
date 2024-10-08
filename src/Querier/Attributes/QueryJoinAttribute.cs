namespace Querier.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class QueryJoinAttribute : BaseAttribute, IJoinAttribute
    {
        public string? JoinRefTable { get; set; }
        public string? JoinRefColumn { get; set; }
        public string? JoinColumn { get; set; }
        public QueryJoinAttribute(string refTable, string refColumn, string column)
        {
            JoinRefTable = refTable;
            JoinRefColumn = refColumn;
            JoinColumn = column;
        }
    }
}
