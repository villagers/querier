using Coravel;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using Querier.Interfaces;
using Querier.Options;
using Querier.Schedules;
using Querier.Schema;
using Querier.SqlQuery;
using Querier.SqlQuery.Functions;
using Querier.SqlQuery.Interfaces;
using Querier.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Extensions
{
    public static class QuerierRegistrationExtension
    {
        public static IServiceCollection AddQuerier(this IServiceCollection services, Action<QuerierRegistrationOption> opt)
        {

            var options = new QuerierRegistrationOption(services);
            opt.Invoke(options);

            services.AddSingleton<SchemaLoader>();
            services.AddSingleton(new SchemaStore()
            {
                LocalStoragePath = options.LocalStoragePath
            });
            services.AddSingleton(new QuerierOption()
            {
                Enabled = options.Enabled,
                LocalStoragePath = options.LocalStoragePath
            });

            services.AddScoped<IQuery, Query>();
            services.AddScoped<IDuckDBQueryBuilder, DuckDBQueryBuilder>();
            services.AddScoped<IQueryDbConnection>(e => new QueryDbConnection(options.Connection()));

            services.TryAddScoped<IMeasurePropertyValidator, MeasurePropertyValidator>();
            services.TryAddScoped<IDimensionPropertyValidator, DimensionPropertyValidator>();
            services.TryAddScoped<ITimeDimensionPropertyValidator, TimeDimensionPropertyValidator>();

            services.AddTransient<QuerySchemaExecutor>();
            services.AddTransient<QuerySchemaDatabase>();
            services.AddTransient<QuerySchemaInitiator>();

            services.AddScheduler();

            return services;
        }

        public static IApplicationBuilder UseQuerier(this IApplicationBuilder app)
        {
            var options = app.ApplicationServices.GetRequiredService<QuerierOption>();
            if (!options.Enabled) return app;

            app.ApplicationServices.GetRequiredService<SchemaLoader>().LoadDefaults();

            var store = app.ApplicationServices.GetRequiredService<SchemaStore>();
            using (var scope = app.ApplicationServices.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<ISchemaSqlGenerator>().Generate();
                scope.ServiceProvider.GetRequiredService<QuerySchemaInitiator>().InitiateAsync().Wait();
            }

            app.ApplicationServices.UseScheduler(e =>
            {
                e.OnWorker("query");
                foreach (var schema in store.Schemas)
                {
                    if (!schema.Initialized) continue;

                    using (var scope = app.ApplicationServices.CreateScope())
                    {    
                        if (schema.WarmUp)
                        {
                            scope.ServiceProvider.GetRequiredService<QuerySchemaExecutor>().Invoke(schema).Wait();
                        }
                    }
                    e.ScheduleWithParams<QuerySchemaScheduler>(schema).Cron(schema.RefreshInterval).PreventOverlapping(schema.Key);
                }
            });


            return app;
        }
    }
}
