using Querier.SqlQuery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Functions
{
    public interface IFunction : IDateFunction
    {
        IFunction New();
        SqlFunctionResult Compile();
    }
}
