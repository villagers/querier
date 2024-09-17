using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Models;
using Querier.SqlQuery.Tokenizers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Operators
{
    public class LikeOperator : AbstractLogicalOperator
    {
        public required object Value { get; set; }
        public required string LikeStarts { get; set; } = "%";
        public required string LikeEnds { get; set; } = "%";
        public override SqlQueryResult Compile(ISqlTable table)
        {
            var column = Column.Compile(table);

            var sqlTz = new SqlTokenizer()
                .AddToken(AndOrOperator)
                .AddToken(column.Sql)
                .AddToken(NotOperator)
                .AddToken("like")
                .AddToken(e => e.AddToken(LikeStarts).AddToken("@value").AddToken(LikeEnds), "")
                .Build();


            var result = new SqlQueryResult()
            {
                Sql = sqlTz,
                NameParameters = column.NameParameters,
                SqlParameters = new Dictionary<string, object>() { { "@value",Value } }
            };
            return result.Enumerate();
        }
    }
}
