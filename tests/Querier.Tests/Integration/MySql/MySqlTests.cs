using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Querier.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Tests.Integration.MySql
{
    public class MySqlTests : IClassFixture<MySqlWebApplicationFactory<Program>>
    {
        private readonly MySqlWebApplicationFactory<Program> _application;
        private readonly IQuery _query;

        public MySqlTests(MySqlWebApplicationFactory<Program> application)
        {
            _application = application;

            var scope = _application.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            _query = scope.ServiceProvider.GetRequiredService<IQuery>();
        }
    }
}
