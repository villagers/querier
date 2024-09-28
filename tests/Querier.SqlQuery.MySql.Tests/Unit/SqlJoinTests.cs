using Querier.SqlQuery.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            const string join1 = "select * from `orders` as `orders` inner join `products` on `products`.`id` = `orders`.`product_id`";
            const string join2 = "select * from `orders` as `orders` inner join `products` on `products`.`id` = `orders`.`product_id` inner join `categories` on `categories`.`id` = `orders`.`category_id`";

            Assert.Equal(join1, _query.New().From("orders").Join("products", "id", "product_id").Compile().CompiledSql);
            Assert.Equal(join2, _query.New().From("orders").Join("products", "id", "product_id").Join("categories", "id", "category_id").Compile().CompiledSql);
        }
    }
}
