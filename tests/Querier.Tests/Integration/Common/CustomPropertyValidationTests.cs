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

namespace Querier.Tests.Integration.Common
{

    public class CustomPropertyValidationWebApplicationFactory<TProgram> : BaseWebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.Configure(app =>
            {
                app.UseQuerier(e => e.Types.LoadDefaults());
            });
            builder.ConfigureTestServices(services =>
            {
                var connectionString = Configuration.GetConnectionString("MySQL");
                services.AddQuerier(o =>
                {
                    o.Validation.MeasureValidator<CustomMeasureValidation>();
                    o.Validation.DimensionValidator<CustomDimensionValidation>();
                    o.Validation.TimeDimensionValidator<CustomTimeDimensionValidation>();
                    o.UseMySql(connectionString);
                });
            });

        }
    }

    class CustomMeasureValidation : IMeasurePropertyValidator
    {
        public bool Validate(PropertyInfo propertyInfo)
        {
            return new[]
            {
                typeof(string)
            }.Contains(propertyInfo.PropertyType);
        }
    }

    class CustomDimensionValidation : IDimensionPropertyValidator
    {
        public bool Validate(PropertyInfo propertyInfo)
        {
            return new[]
            {
                typeof(int)
            }.Contains(propertyInfo.PropertyType);
        }
    }

    class CustomTimeDimensionValidation : ITimeDimensionPropertyValidator
    {
        public bool Validate(PropertyInfo propertyInfo)
        {
            return false;
        }
    }


    public class CustomPropertyValidationTests : IClassFixture<CustomPropertyValidationWebApplicationFactory<Program>>
    {
        private readonly IQuery _query;
        private readonly CustomPropertyValidationWebApplicationFactory<Program> _application;

        public CustomPropertyValidationTests(CustomPropertyValidationWebApplicationFactory<Program> application)
        {
            _application = application;

            var scope = _application.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            _query = scope.ServiceProvider.GetRequiredService<IQuery>();
        }

        [Fact]
        public void CustomMeasureValidationTest()
        {
            var withType = _query.GetMeasures<SampleEntity>();
            var withTypeName = _query.GetMeasures("SampleEntity");

            var withTypeQuery = _query.GetMeasures<SampleEntityQuery>();
            var withTypeNameQuery = _query.GetMeasures("SampleEntityQuery");


            Assert.Equal(2, withType.Count());
            Assert.Equal(2, withTypeName.Count());
            Assert.Equal(2, withTypeQuery.Count());
            Assert.Equal(2, withTypeNameQuery.Count());
        }

        [Fact]
        public void CustomDimensionValidationTest()
        {
            var withType = _query.GetDimensions<SampleEntity>();
            var withTypeName = _query.GetDimensions("SampleEntity");

            var withTypeQuery = _query.GetDimensions<SampleEntityQuery>();
            var withTypeNameQuery = _query.GetDimensions("SampleEntityQuery");


            Assert.Single(withType);
            Assert.Single(withTypeName);
            Assert.Equal(2, withTypeQuery.Count());
            Assert.Equal(2, withTypeNameQuery.Count());
        }

        [Fact]
        public void CustomTimeDimensionValidationTest()
        {
            var withType = _query.GetTimeDimensions<SampleEntity>();
            var withTypeName = _query.GetTimeDimensions("SampleEntity");

            var withTypeQuery = _query.GetTimeDimensions<SampleEntityQuery>();
            var withTypeNameQuery = _query.GetTimeDimensions("SampleEntityQuery");


            Assert.Empty(withType);
            Assert.Empty(withTypeName);
            Assert.Empty(withTypeQuery);
            Assert.Empty(withTypeNameQuery);
        }

    }
}
