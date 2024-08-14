using Querier.SqlQuery.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Providers
{
    public abstract class AbstractProvider<TQuery> where TQuery : IQuery<TQuery>, new()
    {
        public abstract string Compile(TQuery query);
    }
}
