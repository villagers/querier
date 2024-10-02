namespace Querier.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class QueryDescriptionAttribute : BaseAttribute, IDescriptionAttribute
    {
        public string? Description { get; set; }
        public QueryDescriptionAttribute(string description) 
        {
            Description = description;
        }
    }
}
