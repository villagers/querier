namespace Querier.SqlQuery.Interfaces
{
    public interface IRawSqlQuery<TQuery>
    {
        TQuery Raw(string rawSql);
        TQuery AppendRaw(string rawSql);
    }
}
