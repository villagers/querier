using Querier.SqlQuery.Functions;
using Querier.SqlQuery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.MySql.Tests.Unit
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
            const string select1 = "select * from `orders` as `orders`";
            const string select2 = "select `orders`.`id` from `orders` as `orders`";
            const string select3 = "select `orders`.`id`, `orders`.`total` from `orders` as `orders`";
            const string select4 = "select `orders`.`id` as `order_id`, `orders`.`total` as `order_total` from `orders` as `orders`";

            Assert.Equal(select1, _query.New().From("orders").Select().Compile().CompiledSql);
            Assert.Equal(select2, _query.New().From("orders").Select("id").Compile().CompiledSql);
            Assert.Equal(select3, _query.New().From("orders").Select("id").Select("total").Compile().CompiledSql);
            Assert.Equal(select4, _query.New().From("orders").Select("id", "order_id").Select("total", "order_total").Compile().CompiledSql);
        }

        [Fact]
        public void SelectAggregation()
        {
            const string select1 = "select avg(`orders`.`total`) from `orders` as `orders`";
            const string select2 = "select avg(`orders`.`total`) as `avg_total` from `orders` as `orders`";
            const string select3 = "select count(`orders`.`total`) from `orders` as `orders`";
            const string select4 = "select count(`orders`.`total`) as `count_total` from `orders` as `orders`";
            const string select5 = "select max(`orders`.`total`) from `orders` as `orders`";
            const string select6 = "select max(`orders`.`total`) as `max_total` from `orders` as `orders`";
            const string select7 = "select min(`orders`.`total`) from `orders` as `orders`";
            const string select8 = "select min(`orders`.`total`) as `min_total` from `orders` as `orders`";
            const string select9 = "select sum(`orders`.`total`) from `orders` as `orders`";
            const string select10 = "select sum(`orders`.`total`) as `sum_total` from `orders` as `orders`";
            const string select11 = "select `orders`.`id`, avg(`orders`.`total`), sum(`orders`.`total`) as `sum_total` from `orders` as `orders`";


            Assert.Equal(select1, _query.New().From("orders").SelectAvg("total").Compile().CompiledSql);
            Assert.Equal(select2, _query.New().From("orders").SelectAvg("total", "avg_total").Compile().CompiledSql);
            Assert.Equal(select3, _query.New().From("orders").SelectCount("total").Compile().CompiledSql);
            Assert.Equal(select4, _query.New().From("orders").SelectCount("total", "count_total").Compile().CompiledSql);
            Assert.Equal(select5, _query.New().From("orders").SelectMax("total").Compile().CompiledSql);
            Assert.Equal(select6, _query.New().From("orders").SelectMax("total", "max_total").Compile().CompiledSql);
            Assert.Equal(select7, _query.New().From("orders").SelectMin("total").Compile().CompiledSql);
            Assert.Equal(select8, _query.New().From("orders").SelectMin("total", "min_total").Compile().CompiledSql);
            Assert.Equal(select9, _query.New().From("orders").SelectSum("total").Compile().CompiledSql);
            Assert.Equal(select10, _query.New().From("orders").SelectSum("total", "sum_total").Compile().CompiledSql);
            Assert.Equal(select11, _query.New().From("orders").Select("id").SelectAvg("total").SelectSum("total", "sum_total").Compile().CompiledSql);
        }

        [Fact]
        public void SelectDateFunctions()
        {
            const string select1 = "select second(`orders`.`date_created`) from `orders` as `orders`";
            const string select2 = "select second(`orders`.`date_created`) as `sec` from `orders` as `orders`";
            const string select3 = "select minute(`orders`.`date_created`) from `orders` as `orders`";
            const string select4 = "select minute(`orders`.`date_created`) as `min` from `orders` as `orders`";
            const string select5 = "select hour(`orders`.`date_created`) from `orders` as `orders`";
            const string select6 = "select hour(`orders`.`date_created`) as `hr` from `orders` as `orders`";
            const string select7 = "select day(`orders`.`date_created`) from `orders` as `orders`";
            const string select8 = "select day(`orders`.`date_created`) as `d` from `orders` as `orders`";
            const string select9 = "select date(`orders`.`date_created`) from `orders` as `orders`";
            const string select10 = "select date(`orders`.`date_created`) as `dt` from `orders` as `orders`";
            const string select11 = "select month(`orders`.`date_created`) from `orders` as `orders`";
            const string select12 = "select month(`orders`.`date_created`) as `m` from `orders` as `orders`";
            const string select13 = "select year(`orders`.`date_created`) from `orders` as `orders`";
            const string select14 = "select year(`orders`.`date_created`) as `y` from `orders` as `orders`";

            Assert.Equal(select1, _query.New().From("orders").SelectSecond("date_created").Compile().CompiledSql);
            Assert.Equal(select2, _query.New().From("orders").SelectSecond("date_created", "sec").Compile().CompiledSql);
            Assert.Equal(select3, _query.New().From("orders").SelectMinute("date_created").Compile().CompiledSql);
            Assert.Equal(select4, _query.New().From("orders").SelectMinute("date_created", "min").Compile().CompiledSql);

            Assert.Equal(select5, _query.New().From("orders").SelectHour("date_created").Compile().CompiledSql);
            Assert.Equal(select6, _query.New().From("orders").SelectHour("date_created", "hr").Compile().CompiledSql);

            Assert.Equal(select7, _query.New().From("orders").SelectDay("date_created").Compile().CompiledSql);
            Assert.Equal(select8, _query.New().From("orders").SelectDay("date_created", "d").Compile().CompiledSql);

            Assert.Equal(select9, _query.New().From("orders").SelectDate("date_created").Compile().CompiledSql);
            Assert.Equal(select10, _query.New().From("orders").SelectDate("date_created", "dt").Compile().CompiledSql);

            Assert.Equal(select11, _query.New().From("orders").SelectMonth("date_created").Compile().CompiledSql);
            Assert.Equal(select12, _query.New().From("orders").SelectMonth("date_created", "m").Compile().CompiledSql);

            Assert.Equal(select13, _query.New().From("orders").SelectYear("date_created").Compile().CompiledSql);
            Assert.Equal(select14, _query.New().From("orders").SelectYear("date_created", "y").Compile().CompiledSql);
        }
    }
}
