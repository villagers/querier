using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Tests.Integration
{
    public class BaseWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected IConfiguration Configuration { set; get; }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("ci");
            builder.ConfigureAppConfiguration((context, builder) =>
            {
                Configuration = builder.AddEnvironmentVariables().Build();
            });
        }
    }
}
