using Microsoft.Extensions.DependencyInjection;
using Querier.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Tests.Integration.MySql
{
    public class MySqlDimensionTests : IClassFixture<MySqlWebApplicationFactory<Program>>
    {
        private readonly IQuery _query;
        private readonly MySqlWebApplicationFactory<Program> _application;

        public MySqlDimensionTests(MySqlWebApplicationFactory<Program> application)
        {
            _application = application;

            var scope = _application.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            _query = scope.ServiceProvider.GetRequiredService<IQuery>();
        }

        [Fact]
        public void DimensionSingle()
        {
            var result = _query.New().From("Invoice").Dimension("BillingCity").Execute();

            Assert.Empty(result.Measures);
            Assert.Empty(result.TimeDimensions);

            Assert.Single(result.Dimensions);

            Assert.Equal(53, result.Data.Count());
        }

        [Fact]
        public void DimensionMultiple()
        {
            var result = _query.New().From("Invoice").Dimension("BillingCity").Dimension("BillingCountry").Execute();

            Assert.Empty(result.Measures);
            Assert.Empty(result.TimeDimensions);

            Assert.Equal(2, result.Dimensions.Count());

            Assert.Equal(53, result.Data.Count());
        }
    }
}
