using Querier.SqlQuery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Interfaces
{
    public interface ISqlQueryCompile<TResult> where TResult : class
    {
        TResult Compile();
    }
}
