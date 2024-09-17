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
            const string table1 = "select * from `orders` as `orders`";
            const string table2 = "select * from `orders` as `o`";
            const string table3 = "select * from (select `customers`.`fname`, `customers`.`lname` from `customers` as `customers`) as `customers`";
            const string table4 = "select * from (select `customers`.`fname`, `customers`.`lname` from `customers` as `customers`) as `oc`";

            Assert.Equal(table1, _query.New().From("orders").Compile().CompiledSql);
            Assert.Equal(table2, _query.New().From("orders", "o").Compile().CompiledSql);
            Assert.Equal(table3, _query.New().From(q => q.Select("fname").Select("lname").From("customers"), "customers").Compile().CompiledSql);
            Assert.Equal(table4, _query.New().From(q => q.Select("fname").Select("lname").From("customers"), "oc").Compile().CompiledSql);
        }
    }
}
