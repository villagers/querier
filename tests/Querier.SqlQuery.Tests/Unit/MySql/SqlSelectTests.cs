using Querier.SqlQuery.Functions;
using Querier.SqlQuery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Tests.Unit.MySql
{
    public class SqlSelectTests
    {
        private IMySqlQuery _query;

        public SqlSelectTests()
        {
            _query = new MySqlQuery(new MySqlFunction());
        }

        [Fact]
        public void Select()
        {
            Assert.Equal("select * from `orders`", _query.New().From("orders").Select().Compile().CompiledSql);
            Assert.Equal("select `id` from `orders`", _query.New().From("orders").Select("id").Compile().CompiledSql);
            Assert.Equal("select `id`, `total` from `orders`", _query.New().From("orders").Select("id").Select("total").Compile().CompiledSql);
            Assert.Equal("select `id` as `order_id`, `total` as `order_total` from `orders`", _query.New().From("orders").Select("id", "order_id").Select("total", "order_total").Compile().CompiledSql);

            Assert.Equal("select distinct `id`, avg(`total`), sum(`total`) as `sum_total` from `orders`", _query.New().From("orders").Distinct().Select("id").SelectAvg("total").SelectSum("total", "sum_total").Compile().CompiledSql);
        }

        [Fact]
        public void SelectAggregation()
        {
            Assert.Equal("select avg(`total`) from `orders`", _query.New().From("orders").SelectAvg("total").Compile().CompiledSql);
            Assert.Equal("select avg(`total`) as `avg_total` from `orders`", _query.New().From("orders").SelectAvg("total", "avg_total").Compile().CompiledSql);

            Assert.Equal("select count(`total`) from `orders`", _query.New().From("orders").SelectCount("total").Compile().CompiledSql);
            Assert.Equal("select count(`total`) as `count_total` from `orders`", _query.New().From("orders").SelectCount("total", "count_total").Compile().CompiledSql);

            Assert.Equal("select max(`total`) from `orders`", _query.New().From("orders").SelectMax("total").Compile().CompiledSql);
            Assert.Equal("select max(`total`) as `max_total` from `orders`", _query.New().From("orders").SelectMax("total", "max_total").Compile().CompiledSql);

            Assert.Equal("select min(`total`) from `orders`", _query.New().From("orders").SelectMin("total").Compile().CompiledSql);
            Assert.Equal("select min(`total`) as `min_total` from `orders`", _query.New().From("orders").SelectMin("total", "min_total").Compile().CompiledSql);

            Assert.Equal("select sum(`total`) from `orders`", _query.New().From("orders").SelectSum("total").Compile().CompiledSql);
            Assert.Equal("select sum(`total`) as `sum_total` from `orders`", _query.New().From("orders").SelectSum("total", "sum_total").Compile().CompiledSql);

            Assert.Equal("select `id`, avg(`total`), sum(`total`) as `sum_total` from `orders`", _query.New().From("orders").Select("id").SelectAvg("total").SelectSum("total", "sum_total").Compile().CompiledSql);
        }

        [Fact]
        public void SelectDateFunctions()
        {
            Assert.Equal("select second(`date_created`) from `orders`", _query.New().From("orders").SelectSecond("date_created").Compile().CompiledSql);
            Assert.Equal("select second(`date_created`) as `sec` from `orders`", _query.New().From("orders").SelectSecond("date_created", "sec").Compile().CompiledSql);

            Assert.Equal("select minute(`date_created`) from `orders`", _query.New().From("orders").SelectMinute("date_created").Compile().CompiledSql);
            Assert.Equal("select minute(`date_created`) as `min` from `orders`", _query.New().From("orders").SelectMinute("date_created", "min").Compile().CompiledSql);

            Assert.Equal("select hour(`date_created`) from `orders`", _query.New().From("orders").SelectHour("date_created").Compile().CompiledSql);
            Assert.Equal("select hour(`date_created`) as `hr` from `orders`", _query.New().From("orders").SelectHour("date_created", "hr").Compile().CompiledSql);

            Assert.Equal("select day(`date_created`) from `orders`", _query.New().From("orders").SelectDay("date_created").Compile().CompiledSql);
            Assert.Equal("select day(`date_created`) as `d` from `orders`", _query.New().From("orders").SelectDay("date_created", "d").Compile().CompiledSql);

            Assert.Equal("select date(`date_created`) from `orders`", _query.New().From("orders").SelectDate("date_created").Compile().CompiledSql);
            Assert.Equal("select date(`date_created`) as `dt` from `orders`", _query.New().From("orders").SelectDate("date_created", "dt").Compile().CompiledSql);

            Assert.Equal("select month(`date_created`) from `orders`", _query.New().From("orders").SelectMonth("date_created").Compile().CompiledSql);
            Assert.Equal("select month(`date_created`) as `m` from `orders`", _query.New().From("orders").SelectMonth("date_created", "m").Compile().CompiledSql);

            Assert.Equal("select year(`date_created`) from `orders`", _query.New().From("orders").SelectYear("date_created").Compile().CompiledSql);
            Assert.Equal("select year(`date_created`) as `y` from `orders`", _query.New().From("orders").SelectYear("date_created", "y").Compile().CompiledSql);
        }
    }
}
