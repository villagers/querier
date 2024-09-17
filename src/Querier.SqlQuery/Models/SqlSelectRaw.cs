using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Models
{
    public class SqlSelectRaw : ISqlSelect
    {
        public required string RawSql { get; set; }
        public virtual SqlQueryResult Compile(ISqlTable table)
        {
            return new SqlQueryResult()
            {
                Sql = RawSql
            };
        }
    }
}
