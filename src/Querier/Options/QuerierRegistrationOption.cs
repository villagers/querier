using Microsoft.Extensions.DependencyInjection;
using Querier.Interfaces;
using Querier.SqlQuery.Functions;
using Querier.SqlQuery;
using Querier.Schema;
using Microsoft.Extensions.Options;

namespace Querier.Options
{
    public class QuerierRegistrationOption
    {
        private readonly IServiceCollection _services;

        public bool Enabled { get; set; } = true;
        public string LocalStoragePath { set; get; }
        public ValidationOption Validation { private set; get; }

        public QuerierRegistrationOption(IServiceCollection services)
        {
            _services = services;

            Validation = new ValidationOption(services);
        }
        public void UseMySql(string connectionString)
        {
            _services.AddScoped<IFunction, MySqlFunction>();
            _services.AddScoped<IMySqlQueryBuilder, MySqlQueryBuilder>();
            _services.AddScoped<ISchemaSqlGenerator, SchemaSqlGenerator<IMySqlQueryBuilder>>();
            _services.AddScoped<IQueryDbConnection>(e => new QueryDbMySqlConnection(connectionString));
        }
    }
}
