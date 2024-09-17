using Querier.SqlQuery.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Models
{
    public class SqlGroupByRaw : ISqlGroupBy
    {
        public required string RawSql { get; set; }
        public SqlQueryResult Compile(ISqlTable table)
        {
            return new SqlQueryResult()
            {
                Sql = RawSql
            };
        }
    }
}
