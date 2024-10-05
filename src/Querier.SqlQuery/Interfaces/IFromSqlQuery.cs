namespace Querier.SqlQuery.Interfaces
{
    public interface IFromSqlQuery<TQuery>
    {
        TQuery FromRaw(string sql);
        TQuery From(string table, string? tableAs = null);
        TQuery From(Func<TQuery, TQuery> query, string tableAs);
    }
}
