namespace Querier.SqlQuery.Operators
{
    public abstract class AbstractComparisonOperator<T> : AbstractOperator
    {
        public T Value { get; set; }
    }
}
