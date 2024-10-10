namespace Querier.SqlQuery.Interfaces
{
    public interface IJoinSqlQuery<TQuery>
    {
        TQuery JoinRaw(string referenceTable, string rawSql);
        TQuery Join(string column, string referenceTable, string referenceColumn);
        TQuery InnerJoin(string column, string referenceTable, string referenceColumn);
        TQuery LeftJoin(string column, string referenceTable, string referenceColumn);
        TQuery RightJoin(string column, string referenceTable, string referenceColumn);
        TQuery FullJoin(string column, string referenceTable, string referenceColumn);
        TQuery CrossJoin(string column, string referenceTable, string referenceColumn);
        TQuery CrossJoinInline<T>(IEnumerable<T> values, string column, string tableAs);


        TQuery AndOn(string column, string referenceColumn);
        TQuery AndOn<T>(string table, string column, T columnValue);
    }
}
