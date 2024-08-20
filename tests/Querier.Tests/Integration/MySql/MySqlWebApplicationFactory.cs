using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Querier.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Tests.Integration.MySql
{
    public class MySqlWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        private IConfiguration Configuration { set; get; }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("mysql");
            builder.ConfigureAppConfiguration((context, builder) =>
            {
                Configuration = builder.Build();
            });

            builder.ConfigureServices(services =>
            {
                var connectionString = Configuration.GetConnectionString("Default");
                services.AddQuerier(o => o.UseMySql(connectionString));
            });
        }
    }
}
