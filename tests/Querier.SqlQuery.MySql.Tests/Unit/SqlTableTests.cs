using Querier.SqlQuery.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.MySql.Tests.Unit
{
    public class SqlTableTests
    {
        private IMySqlQuery _query;

        public SqlTableTests()
        {
            _query = new MySqlQuery(new MySqlFunction());
        }

        [Fact]
        public void Table()
        {
            Assert.Equal("select * from `orders`", _query.New().From("orders").Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `o`", _query.New().From("orders", "o").Compile().CompiledSql);
            Assert.Equal("select * from (select `fname`, `lname` from `customers`)", _query.New().From(q => q.Select("fname").Select("lname").From("customers")).Compile().CompiledSql);
            Assert.Equal("select * from (select `fname`, `lname` from `customers`) as `oc`", _query.New().From(q => q.Select("fname").Select("lname").From("customers"), "oc").Compile().CompiledSql);
        }
    }
}
