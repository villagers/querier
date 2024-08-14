using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Providers;
using Querier.SqlQuery.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Querier.SqlQuery.Tests
{
    public class SelectTests
    {
        private readonly ITestOutputHelper _outputHelper;

        public SelectTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public void Select()
        {
            IMySqlQuery query = new MySqlQuery();
            query.From("").Limit(1);

            var ss = query.From("order").Select("value").SelectSum("total", "total_sales").Compile();

            Assert.Equal("select * from @n0", query.From("orders").Compile().Sql);
            Assert.Equal("select * from @n0", query.From("orders").Select().Compile().Sql);
            Assert.Equal("select * from @n0", query.From("orders").Select("*").Compile().Sql);




            //query.From("orders");
            query.From(e => e.From("categories").Select("name"));
            //var q1 = query.Compile();

            //var sss = new DefaultProvider().Compile(query);

            



            var res2 = query.From("SnapshotFinancial").Select("ReitId", "r").Select("ReitId").Where("ReitId", 10).Or(15).Between(1, 2).Compile();




            query.From(q => q.From("SnapshotReit").Select("ReitId").Select("ReitValue"));

            query.Select();
            Assert.Equal("select * from SnapshotFinancial", query.Compile().Sql);

            query.Select("Id");
            Assert.Equal("select Id from SnapshotFinancial", query.Compile().Sql);

            query.Select("ReitId", "Reit");
            Assert.Equal("select Id, ReitId as Reit from SnapshotFinancial", query.Compile().Sql);

            query.Select("count", "ReitId", "ReitCount");
            Assert.Equal("select Id, ReitId as Reit, count(ReitId) as ReitCount from SnapshotFinancial", query.Compile().Sql);

            query.Select("sum", "ReitId", "ReitSum");
            Assert.Equal("select Id, ReitId as Reit, count(ReitId) as ReitCount, sum(ReitId) as ReitSum from SnapshotFinancial", query.Compile().Sql);

            query.Select("avg", "ReitId", "ReitAvg");
            Assert.Equal("select Id, ReitId as Reit, count(ReitId) as ReitCount, sum(ReitId) as ReitSum, avg(ReitId) as ReitAvg from SnapshotFinancial", query.Compile().Sql);

            query.Select("min", "ReitId", "ReitMin");
            Assert.Equal("select Id, ReitId as Reit, count(ReitId) as ReitCount, sum(ReitId) as ReitSum, avg(ReitId) as ReitAvg, min(ReitId) as ReitMin from SnapshotFinancial", query.Compile().Sql);

            query.Select("max", "ReitId", "ReitMax");
            Assert.Equal("select Id, ReitId as Reit, count(ReitId) as ReitCount, sum(ReitId) as ReitSum, avg(ReitId) as ReitAvg, min(ReitId) as ReitMin, max(ReitId) as ReitMax from SnapshotFinancial", query.Compile().Sql);

        }
    }
}
