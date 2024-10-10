using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Tokenizers;

namespace Querier.SqlQuery.Models
{
    public class SqlSelectRaw : ISqlSelect
    {
        public required string RawSql { get; set; }
        public string? RawSqlAs { get; set; }
        public virtual SqlQueryResult Compile(ISqlTable table)
        {
            var result = new SqlQueryResult();
            var tokenizer = new SqlTokenizer($"{RawSql}");

            if (!string.IsNullOrWhiteSpace(RawSqlAs))
            {
                tokenizer.AddToken("as").AddToken("@as");
                result.NameParameters.Add("@as", RawSqlAs);
            }
            result.Sql = tokenizer.Build();
            return result;
        }
    }
}
