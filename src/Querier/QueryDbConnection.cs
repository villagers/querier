using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier
{
    public class QueryDbConnection : IQueryDbConnection
    {
        private readonly IDbConnection _dbConnection;

        public QueryDbConnection(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public IDbConnection Connection => _dbConnection;
    }
}
