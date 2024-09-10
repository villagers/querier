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
            Assert.Equal("select * from `orders` group by `product`", _query.New().From("orders").GroupBy("product").Compile().CompiledSql);
            Assert.Equal("select * from `orders` group by `product`, `category`", _query.New().From("orders").GroupBy("product").GroupBy("category").Compile().CompiledSql);
        }
    }
}
