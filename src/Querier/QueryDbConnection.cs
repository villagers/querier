using System.Data;
using MySql.Data.MySqlClient;
using Querier.Interfaces;

namespace Querier
{
    public abstract class QueryDbConnection
    {
        protected readonly string _connectionString;

        public QueryDbConnection(string connectionString)
        {
            _connectionString = connectionString;

        }
    }

    public class QueryDbMySqlConnection : QueryDbConnection, IQueryDbConnection
    {
        public QueryDbMySqlConnection(string connectionString) : base(connectionString) { }

        public IDbConnection Connection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}