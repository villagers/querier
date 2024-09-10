using Querier.SqlQuery.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.MySql.Tests.Unit
{
    public class OrderByTests
    {
        private IMySqlQuery _query;

        public OrderByTests()
        {
            _query = new MySqlQuery(new MySqlFunction());
        }

        [Fact]
        public void OrderBy()
        {
            Assert.Equal("select * from `orders` order by `product_id` asc", _query.New().From("orders").OrderBy("product_id").Compile().CompiledSql);
            Assert.Equal("select * from `orders` order by `product_id` asc, `category_id` desc", _query.New().From("orders").OrderBy("product_id").OrderBy("category_id", "desc").Compile().CompiledSql);
        }
    }
}
