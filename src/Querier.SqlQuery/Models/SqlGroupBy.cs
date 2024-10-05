using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Tokenizers;

namespace Querier.SqlQuery.Models
{
    public class SqlGroupBy : ISqlGroupBy
    {
        public string Column { get; set; }
        public int? OrderId { get; set; }

        public virtual SqlQueryResult Compile(ISqlTable table)
        {
            var result = new SqlQueryResult();
            var selectTz = new SqlTokenizer();

            if (OrderId.HasValue)
            {
                selectTz.AddToken(OrderId.Value.ToString());
            } else
            {
                selectTz.AddToken(e => e.AddToken("@table").AddToken(".").AddToken("@column"), "");
                result.NameParameters.Add("@table", table.TableOrAlias);
                result.NameParameters.Add("@column", Column);
            }

            result.Sql = selectTz.Build();
            return result.Enumerate();
        }
    }
}
