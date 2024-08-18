using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Models
{
    public class SqlOrderBy
    {
        public required string Column { get; set; }
        public required string Order { get; set; }
    }
}
