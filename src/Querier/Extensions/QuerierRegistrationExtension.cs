using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using Querier.SqlQuery;
using Querier.SqlQuery.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Extensions
{
    public class QuerierRegistrationOption
    {
        public IDbConnection DbConnection;
        public readonly IServiceCollection Services;

        public QuerierRegistrationOption(IServiceCollection services)
        {
            Services = services;
        }

        public void UseMySql(string connectionString)
        {
            DbConnection = new MySqlConnection(connectionString);

            Services.AddScoped<IMySqlQuery, MySqlQuery>();
            Services.AddScoped<IQuery, Query<IMySqlQuery>>();
        }
    }
    public static class QuerierRegistrationExtension
    {
        public static IServiceCollection AddQuerier(this IServiceCollection services, Action<QuerierRegistrationOption> opt)
        {
            var options = new QuerierRegistrationOption(services);
            opt.Invoke(options);


            services.AddScoped<IQueryDbConnection>(e => new QueryDbConnection(options.DbConnection));
            return services;
        }
    }
}
