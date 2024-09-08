using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Models
{
    public class SqlSelect : ISqlQueryCompile<SqlQueryResult>
    {
        public SqlColumn? SqlColumn { get; set; }
        public virtual SqlQueryResult Compile() => SqlColumn.Compile().Enumerate();
    }
}
