using System.Data;

namespace Querier.Interfaces
{
    public interface IQueryDbConnection
    {
        public IDbConnection Connection();
    }
}
