using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Tokenizers;

namespace Querier.SqlQuery.Models
{
    public class SqlJoinInline<T> : ISqlJoin
    {
        public string Join { get; set; } = "cross join";
        public required string Column {  get; set; }
        public required string TableAlias { get; set; }
        public required IEnumerable<T> Values { get; set; }

        public SqlQueryResult Compile(ISqlTable table)
        {
            var result = new SqlQueryResult();
            result.NameParameters.Add("@as", TableAlias);
            result.NameParameters.Add("@column", Column);

            var tz = new SqlTokenizer()
                .AddToken(Join)
                .AddToken(e =>
                {
                    e.AddToken("(");
                    e.AddToken("values");

                    var valueList = Values.Select((e, index) =>
                    {
                        result.SqlParameters.Add($"@value{index}", e);
                        return $"(@value{index})";
                    });
                    e.AddToken(string.Join(",", valueList));
                    e.AddToken(")");

                    return e;
                }).AddToken("as").AddToken(e => e.AddToken("@as").AddToken("(").AddToken("@column").AddToken(")"), "");


            result.Sql = tz.Build();
            return result.Enumerate();
        }

        public ISqlJoin On(string column, string referenceColumn)
        {
            throw new NotImplementedException();
        }

        public ISqlJoin On<T1>(string table, string column, T1 columnValue)
        {
            throw new NotImplementedException();
        }
    }
}
