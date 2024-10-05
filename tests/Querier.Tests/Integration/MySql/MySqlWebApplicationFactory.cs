using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Querier.Extensions;
using Querier.Web.Tests.Shared.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Tests.Integration.MySql
{
    public class MySqlWebApplicationFactory<TProgram> : BaseWebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.ConfigureServices(services =>
            {
                var connectionString = Configuration.GetConnectionString("MySQL");
                services.AddQuerier(o =>
                {
                    o.UseMySql(connectionString);
                    o.LocalStoragePath = "/home";
                });
            });
        }
    }
}
