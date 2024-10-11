using Querier.SqlQuery.Functions;

namespace Querier.SqlQuery.MySql.Tests.Unit
{
    public class SqlJoinTests
    {
        private IMySqlQueryBuilder _query;

        public SqlJoinTests()
        {
            _query = new MySqlQueryBuilder(new MySqlFunction());
        }

        [Fact]
        public void OrderBy()
        {
            const string join1 = "select * from `orders` as `orders` inner join `products` on `orders`.`product_id` = `products`.`id`";
            const string join2 = "select * from `orders` as `orders` inner join `products` on `orders`.`product_id` = `products`.`id` inner join `categories` on `orders`.`category_id` = `categories`.`id`";

            Assert.Equal(join1, _query.New().From("orders").Select().Join("product_id", "products", "id").Compile().CompiledSql);
            Assert.Equal(join2, _query.New().From("orders").Select().Join("product_id", "products", "id").Join("category_id", "categories", "id").Compile().CompiledSql);
        }
    }
}
