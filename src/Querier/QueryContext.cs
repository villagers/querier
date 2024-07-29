using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier
{
    public interface IQueryContext<TContext> where TContext : DbContext
    {
        public DbContext GetContext();
    }
    public class QueryContext<TContext> : IQueryContext<TContext> where TContext : DbContext
    {
        private readonly TContext _dbContext;

        public QueryContext(TContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DbContext GetContext() => _dbContext;
    }
}
