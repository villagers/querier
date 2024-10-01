using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using Querier.Interfaces;
using Querier.SqlQuery.Functions;
using Querier.SqlQuery;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Querier.Schema;

namespace Querier.Options
{
    public class QuerierRegistrationOption
    {
        private IDbConnection _dbConnection;
        private readonly IServiceCollection _services;

        public bool Enabled { get; set; } = true;
        public string LocalStoragePath { set; get; }
        public ValidationOption Validation { private set; get; }

        public QuerierRegistrationOption(IServiceCollection services)
        {
            _services = services;

            Validation = new ValidationOption(services);
        }

        public IDbConnection Connection() => _dbConnection;

        public void UseMySql(string connectionString)
        {
            _dbConnection = new MySqlConnection(connectionString);

            _services.AddScoped<IFunction, MySqlFunction>();
            _services.AddScoped<IMySqlQueryBuilder, MySqlQueryBuilder>();
            

            _services.AddScoped<ISchemaSqlGenerator, SchemaSqlGenerator<IMySqlQueryBuilder>>();
        }
    }
}
