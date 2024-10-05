namespace Querier.Interfaces
{
    public interface IQueryExecute
    {
        QueryResult Execute();

        IEnumerable<Dictionary<string, object>>? Get();
    }
}
