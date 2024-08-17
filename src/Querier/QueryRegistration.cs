using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier
{
    public static class QueryRegistration
    {
        public static void AddQuery<TContext>(this IServiceCollection services) where TContext : DbContext
        {
            services.AddScoped(typeof(IQuery<>), typeof(Query<>));
            //services.AddScoped<IQueryContext<TContext>, QueryContext<TContext>>();
        }
    }
}
