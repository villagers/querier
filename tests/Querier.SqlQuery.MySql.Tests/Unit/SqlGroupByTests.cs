using Querier.SqlQuery.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.MySql.Tests.Unit
{
    public class SqlGroupByTests
    {
        private IMySqlQuery _query;

        public SqlGroupByTests()
        {
            _query = new MySqlQuery(new MySqlFunction());
        }

        [Fact]
        public void GroupBy()
        {
            const string groupBy1 = "select * from `orders` as `orders` group by `orders`.`product`";
            const string groupBy2 = "select * from `orders` as `orders` group by `orders`.`product`, `orders`.`category`";

            Assert.Equal(groupBy1, _query.New().From("orders").GroupBy("product").Compile().CompiledSql);
            Assert.Equal(groupBy2, _query.New().From("orders").GroupBy("product").GroupBy("category").Compile().CompiledSql);
        }
    }
}
