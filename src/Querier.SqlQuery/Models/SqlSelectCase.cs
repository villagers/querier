using Querier.SqlQuery.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Models
{
    public class SqlSelectCase : SqlSelect
    {
        public required SqlCase SqlCase { get; set; }
        public override SqlQueryResult Compile() => SqlCase.Compile().Enumerate();
    }
}
