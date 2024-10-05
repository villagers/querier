namespace Querier.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class QueryColumnAttribute : BaseAttribute, IColumnAttribute
    {
        public string? Column { get; set; }
        public QueryColumnAttribute() { }
        public QueryColumnAttribute(string column)
        {
            Column = column;
        }
    }
}
