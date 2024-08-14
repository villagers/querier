using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Tls;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Querier.Tests
{

    public class TestDbContext : DbContext { }
    public class TestType { }

    [TestClass]
    public class UnitTest1
    {
        private readonly IQuery _query;

        public UnitTest1()
        {
            var services = new ServiceCollection();
            services.AddScoped<TestDbContext>();
            services.AddScoped(typeof(IQuery), typeof(Query));
            var serviceProvider = services.BuildServiceProvider();

            _query = serviceProvider.GetRequiredService<IQuery>();
        }

        [TestMethod]
        public void TestMethod1()
        {
            var connectionString = "server=192.168.1.100;port=3306;user=root;password=2sMTFqG7heT8X7u;database=kolytics-staging;SslMode=none";
            using (var connection = new MySqlConnection(connectionString))
            {
                var query = _query.Create<TestType>()
                    .Measure("ISRevenueGri")
                    .Dimension("ReitId")
                    .TimeDimension("DateSnapshot")
                    .Execute();


                // Create a query that retrieves all authors"    
                //var sql = "SELECT * FROM Authors";
                //// Use the Query method to execute the query and return a list of objects
                //List<Author> authors = connection.Query<Author>(sql).ToList();
            }
        }
    }
}