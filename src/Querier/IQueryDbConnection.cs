using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier
{
    public interface IQueryDbConnection
    {
        public IDbConnection Connection { get; }
    }
}
