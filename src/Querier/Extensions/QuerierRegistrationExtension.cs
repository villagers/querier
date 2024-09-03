using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using Querier.Interfaces;
using Querier.Options;
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

            services.AddScoped<IQueryDbConnection>(e => new QueryDbConnection(options.Connection()));

            services.TryAddScoped<IMeasurePropertyValidator, MeasurePropertyValidator>();
            services.TryAddScoped<IDimensionPropertyValidator, DimensionPropertyValidator>();
            services.TryAddScoped<ITimeDimensionPropertyValidator, TimeDimensionPropertyValidator>();

            services.AddScoped<IPropertyMapper, PropertyMapper>();
            services.AddSingleton<IndexStore>();

            return services;
        }

        public static IApplicationBuilder UseQuerier(this IApplicationBuilder app)
        {
            var mapper = app.ApplicationServices.GetRequiredService<IPropertyMapper>();

            var options = new QuerierUseOption(mapper).Types.LoadDefaults();

            return app;
        }
        public static IApplicationBuilder UseQuerier(this IApplicationBuilder app, Action<QuerierUseOption> opt)
        {
            var mapper = app.ApplicationServices.GetRequiredService<IPropertyMapper>();

            var options = new QuerierUseOption(mapper);
            opt.Invoke(options);

            return app;
        }
    }
}
