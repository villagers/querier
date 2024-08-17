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
        private readonly IServiceCollection _services;

        public QuerierRegistrationOption(IServiceCollection services)
        {
            _services = services;
        }

        public void UseMySql(string connectionString)
        {
            DbConnection = new MySqlConnection(connectionString);

            _services.AddScoped<IMySqlQuery, MySqlQuery>();
            _services.AddScoped<IQuery, Query<MySqlQuery>>();
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
