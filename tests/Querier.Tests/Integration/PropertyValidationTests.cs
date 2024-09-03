using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Querier.Attributes;
using Querier.Extensions;
using Querier.Helpers;
using Querier.Interfaces;
using Querier.Tests.Integration.MySql;
using Querier.Tests.Shared;
using System.Reflection;

namespace Querier.Tests.Integration
{
    public class PropertyValidationWebApplicationFactory<TProgram> : BaseWebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.Configure(app =>
            {
                app.UseQuerier();
            });
            builder.ConfigureTestServices(services =>
            {
                var connectionString = Configuration.GetConnectionString("MySQL");
                services.AddQuerier(o => o.UseMySql(connectionString));
            });

        }
    }



    public class PropertyValidationTests : IClassFixture<PropertyValidationWebApplicationFactory<Program>>
    {
        private readonly IQuery _query;
        private readonly PropertyValidationWebApplicationFactory<Program> _application;

        public PropertyValidationTests(PropertyValidationWebApplicationFactory<Program> application)
        {
            _application = application;

            var scope = _application.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            _query = scope.ServiceProvider.GetRequiredService<IQuery>();
        }

        [Fact]
        public void MeasureValidationTest()
        {
            var withType = _query.GetMeasures<SampleEntity>();
            var withTypeName = _query.GetMeasures("SampleEntity");

            var withTypeQuery = _query.GetMeasures<SampleEntityQuery>();
            var withTypeNameQuery = _query.GetMeasures("SampleEntityQuery");


            Assert.Equal(5, withType.Count());
            Assert.Equal(5, withTypeName.Count());
            Assert.Equal(2, withTypeQuery.Count());
            Assert.Equal(2, withTypeNameQuery.Count());
        }

        [Fact]
        public void DimensionValidationTest()
        {
            var withType = _query.GetDimensions<SampleEntity>();
            var withTypeName = _query.GetDimensions("SampleEntity");

            var withTypeQuery = _query.GetDimensions<SampleEntityQuery>();
            var withTypeNameQuery = _query.GetDimensions("SampleEntityQuery");


            Assert.Equal(5, withType.Count());
            Assert.Equal(5, withTypeName.Count());
            Assert.Equal(2, withTypeQuery.Count());
            Assert.Equal(2, withTypeNameQuery.Count());
        }

        [Fact]
        public void TimeDimensionValidationTest()
        {
            var withType = _query.GetTimeDimensions<SampleEntity>();
            var withTypeName = _query.GetTimeDimensions("SampleEntity");

            var withTypeQuery = _query.GetTimeDimensions<SampleEntityQuery>();
            var withTypeNameQuery = _query.GetTimeDimensions("SampleEntityQuery");


            Assert.Single(withType);
            Assert.Single(withTypeName);
            Assert.Single(withTypeQuery);
            Assert.Single(withTypeNameQuery);
        }

    }
}
