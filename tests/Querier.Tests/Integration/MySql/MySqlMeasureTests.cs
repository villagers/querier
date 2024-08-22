using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Querier.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Tests.Integration.MySql
{
    public class MySqlMeasureTests : IClassFixture<MySqlWebApplicationFactory<Program>>
    {
        private readonly MySqlWebApplicationFactory<Program> _application;
        private readonly IQuery _query;

        public MySqlMeasureTests(MySqlWebApplicationFactory<Program> application)
        {
            _application = application;

            var scope = _application.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            _query = scope.ServiceProvider.GetRequiredService<IQuery>();
        }

        [Fact]
        public void MeasureSum()
        {
            var result = _query.New().From("Invoice").MeasureSum("Total").Execute();

            Assert.Empty(result.Dimensions);
            Assert.Empty(result.TimeDimensions);

            Assert.Single(result.Measures);
            Assert.Equal("Total", result.Measures.First().Key);
            Assert.Equal("Total", result.Measures.First().DisplayName);

            Assert.Single(result.Data);
            Assert.Equal(2328.60, Convert.ToDouble(result.Data.First()["sum(`Total`)"]));
        }

        [Fact]
        public void MeasureAvg()
        {
            var result = _query.New().From("Invoice").MeasureAvg("Total").Execute();

            Assert.Empty(result.Dimensions);
            Assert.Empty(result.TimeDimensions);

            Assert.Single(result.Measures);
            Assert.Equal("Total", result.Measures.First().Key);
            Assert.Equal("Total", result.Measures.First().DisplayName);

            Assert.Single(result.Data);
            Assert.Equal(5.651942, Convert.ToDouble(result.Data.First()["avg(`Total`)"]));
        }

        [Fact]
        public void MeasureCount()
        {
            var result = _query.New().From("Invoice").MeasureCount("Total").Execute();

            Assert.Empty(result.Dimensions);
            Assert.Empty(result.TimeDimensions);

            Assert.Single(result.Measures);
            Assert.Equal("Total", result.Measures.First().Key);
            Assert.Equal("Total", result.Measures.First().DisplayName);

            Assert.Single(result.Data);
            Assert.Equal(412, Convert.ToDouble(result.Data.First()["count(`Total`)"]));
        }

        [Fact]
        public void MeasureMax()
        {
            var result = _query.New().From("Invoice").MeasureMax("Total").Execute();

            Assert.Empty(result.Dimensions);
            Assert.Empty(result.TimeDimensions);

            Assert.Single(result.Measures);
            Assert.Equal("Total", result.Measures.First().Key);
            Assert.Equal("Total", result.Measures.First().DisplayName);

            Assert.Single(result.Data);
            Assert.Equal(25.86, Convert.ToDouble(result.Data.First()["max(`Total`)"]));
        }

        [Fact]
        public void MeasureMin()
        {
            var result = _query.New().From("Invoice").MeasureMin("Total").Execute();

            Assert.Empty(result.Dimensions);
            Assert.Empty(result.TimeDimensions);

            Assert.Single(result.Measures);
            Assert.Equal("Total", result.Measures.First().Key);
            Assert.Equal("Total", result.Measures.First().DisplayName);

            Assert.Single(result.Data);
            Assert.Equal(0.99, Convert.ToDouble(result.Data.First()["min(`Total`)"]));
        }

        [Fact]
        public void MeasureMixed()
        {
            var result = _query.New().From("Invoice")
                .MeasureSum("Total")
                .MeasureAvg("Total")
                .MeasureCount("Total")
                .MeasureMax("Total")
                .MeasureMin("Total")
                .Execute();

            Assert.Empty(result.Dimensions);
            Assert.Empty(result.TimeDimensions);

            Assert.Equal(5, result.Measures.Count());
            Assert.Equal(5, result.Data.First().Count());
        }
    }
}
