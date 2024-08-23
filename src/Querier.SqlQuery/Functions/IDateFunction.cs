using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Functions
{
    public interface IDateFunction
    {
        IFunction Date(string column);
        IFunction Year(string column);
        IFunction Month(string column);
        IFunction Day(string column);
        IFunction Hour(string column);
        IFunction Minute(string column);
        IFunction Second(string column);
    }
}
