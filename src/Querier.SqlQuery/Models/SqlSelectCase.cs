using Querier.SqlQuery.Extensions;
using Querier.SqlQuery.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Models
{
    public class SqlSelectCase : ISqlSelect
    {
        public required SqlCase SqlCase { get; set; }
        public SqlQueryResult Compile() => SqlCase.Compile().Enumerate();
    }
}
