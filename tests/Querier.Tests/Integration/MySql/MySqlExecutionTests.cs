using Microsoft.Extensions.DependencyInjection;
using Querier.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Tests.Integration.MySql
{
    public class MySqlExecutionTests : IClassFixture<MySqlWebApplicationFactory<Program>>
    {
        private readonly MySqlWebApplicationFactory<Program> _application;
        private readonly IQuery _query;

        public MySqlExecutionTests(MySqlWebApplicationFactory<Program> application)
        {
            _application = application;

            var scope = _application.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            _query = scope.ServiceProvider.GetRequiredService<IQuery>();
        }

        [Fact]
        public void GetScalarTests()
        {
            var result = _query.New().From("Invoice").MeasureSum("Total").GetScalar();
            Assert.Equal(2328.60m, result);

            result = _query.New().From("Invoice").MeasureSum("Total").GetScalarAsync().Result;
            Assert.Equal(2328.60m, result);

            result = _query.New().From("Invoice").MeasureSum("Total").GetScalar<decimal>();
            Assert.Equal(2328.60m, result);

            result = _query.New().From("Invoice").MeasureSum("Total").GetScalarAsync<decimal>().Result;
            Assert.Equal(2328.60m, result);

        }

        [Fact]
        public void GetSingleTests()
        {
            var result = _query.New().From("Invoice").MeasureSum("Total").GetSingle();
            Assert.Single(result);

            result = _query.New().From("Invoice").MeasureSum("Total").GetSingleAsync().Result;
            Assert.Single(result);

            var value = _query.New().From("Invoice").MeasureSum("Total").GetSingleValue();
            Assert.Equal(2328.60m, value);

            value = _query.New().From("Invoice").MeasureSum("Total").MeasureCount("CustomerId").GetSingleValue("Total");
            Assert.Equal(2328.60m, value);


            value = _query.New().From("Invoice").MeasureSum("Total").GetSingleValueAsync().Result;
            Assert.Equal(2328.60m, value);

        }

        [Fact]
        public void GetFirstTests()
        {
            Assert.Single(_query.New().From("Invoice").MeasureSum("Total").GetFirst());
            Assert.Single(_query.New().From("Invoice").MeasureSum("Total").GetFirstAsync().Result);
            Assert.Equal(2328.60m, _query.New().From("Invoice").MeasureSum("Total").GetFirstValue());
            Assert.Equal(2328.60m, _query.New().From("Invoice").MeasureSum("Total").MeasureCount("CustomerId").GetFirstValue("Total"));
            Assert.Equal(2328.60m, _query.New().From("Invoice").MeasureSum("Total").GetFirstValueAsync().Result);

        }
    }
}
