using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Interfaces
{
    public interface IJoinSqlQuery<TQuery>
    {
        TQuery Join(string table, string tableProperty, string property);
    }
}
