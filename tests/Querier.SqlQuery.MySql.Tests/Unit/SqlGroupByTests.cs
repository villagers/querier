using Querier.SqlQuery.Functions;

namespace Querier.SqlQuery.MySql.Tests.Unit
{
    public class SqlGroupByTests
    {
        private IMySqlQueryBuilder _query;

        public SqlGroupByTests()
        {
            _query = new MySqlQueryBuilder(new MySqlFunction());
        }

        [Fact]
        public void GroupBy()
        {
            const string groupBy1 = "select * from `orders` as `orders` group by `orders`.`product`";
            const string groupBy2 = "select * from `orders` as `orders` group by `orders`.`product`, `orders`.`category`";

            Assert.Equal(groupBy1, _query.New().Select().From("orders").GroupBy("product").Compile().CompiledSql);
            Assert.Equal(groupBy2, _query.New().Select().From("orders").GroupBy("product").GroupBy("category").Compile().CompiledSql);
        }
    }
}
