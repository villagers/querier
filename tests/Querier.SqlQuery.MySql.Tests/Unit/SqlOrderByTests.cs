using Querier.SqlQuery.Functions;
using Querier.SqlQuery.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.MySql.Tests.Unit
{
    public class SqlOrderByTests
    {
        private IMySqlQueryBuilder _query;

        public SqlOrderByTests()
        {
            _query = new MySqlQueryBuilder(new MySqlFunction());
        }

        [Fact]
        public void OrderBy()
        {
            const string orderBy1 = "select * from `orders` as `orders` order by `orders`.`product_id` asc";
            const string orderBy2 = "select * from `orders` as `orders` order by `orders`.`product_id` asc, `orders`.`category_id` desc";

            Assert.Equal(orderBy1, _query.New().From("orders").OrderBy("product_id").Compile().CompiledSql);
            Assert.Equal(orderBy2, _query.New().From("orders").OrderBy("product_id").OrderBy("category_id", "desc").Compile().CompiledSql);
        }
    }
}
