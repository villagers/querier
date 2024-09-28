using Coravel;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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

            services.AddScoped<IQuery, Query>();
            services.AddScoped<IDuckDBQueryBuilder, DuckDBQueryBuilder>();

            services.AddScoped<IQueryDbConnection>(e => new QueryDbConnection(options.Connection()));

            services.TryAddScoped<IMeasurePropertyValidator, MeasurePropertyValidator>();
            services.TryAddScoped<IDimensionPropertyValidator, DimensionPropertyValidator>();
            services.TryAddScoped<ITimeDimensionPropertyValidator, TimeDimensionPropertyValidator>();

            services.AddSingleton<SchemaLoader>();
            services.AddSingleton(new SchemaStore()
            {
                LocalStoragePath = options.LocalStoragePath
            });
            

            services.AddScheduler();
            services.AddScoped<SchemaScheduler>();

            services.AddTransient<QuerySchemaInitiator>();

            return services;
        }

        public static IApplicationBuilder UseQuerier(this IApplicationBuilder app)
        {

            app.ApplicationServices.GetRequiredService<SchemaLoader>().LoadDefaults();

            var store = app.ApplicationServices.GetRequiredService<SchemaStore>();
            using (var scope = app.ApplicationServices.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<ISchemaSqlGenerator>().Generate();

                app.ApplicationServices.UseScheduler(e =>
                {
                    foreach (var schema in store.Schemas)
                    {
                        scope.ServiceProvider.GetRequiredService<QuerySchemaInitiator>().InitiateAsync().Wait();
                        if (schema.RunOnceAtStart)
                        {
                            var executor = new QuerySchemaExecutor(store, scope.ServiceProvider.GetRequiredService<IQueryDbConnection>(), schema)
                            {
                                Payload = schema
                            };
                            executor.Invoke().Wait();
                        }
                        e.ScheduleWithParams<QuerySchemaExecutor>(schema).Cron(schema.RefreshInterval).PreventOverlapping(schema.Key);
                    }
                }).OnError(e =>
                {
                    var err = e;
                });
                //scope.ServiceProvider.GetRequiredService<SchemaScheduler>().Schedule();
            }


            return app;
        }
        public static IApplicationBuilder UseQuerier(this IApplicationBuilder app, Action<SchemaLoader> opt)
        {
            var options = new SchemaLoader(app.ApplicationServices.GetRequiredService<SchemaStore>(), app.ApplicationServices);
            opt.Invoke(options);

            return app;
        }
    }
}
