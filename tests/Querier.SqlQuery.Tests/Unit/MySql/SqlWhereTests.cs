using Querier.SqlQuery.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Tests.Unit.MySql
{
    public class SqlWhereTests
    {
        private IMySqlQuery _query;

        public SqlWhereTests()
        {
            _query = new MySqlQuery(new MySqlFunction());
        }

        [Fact]
        public void Where()
        {
            Assert.Equal("select * from `orders` where `id` = @p0", _query.New().From("orders").Where("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` = @p0", _query.New().From("orders").WhereEqual("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` = @p0", _query.New().From("orders").Where("id").Equal(1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` != @p0", _query.New().From("orders").WhereNotEqual("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` != @p0", _query.New().From("orders").Where("id").NotEqual(1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` > @p0", _query.New().From("orders").WhereGreater("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` > @p0", _query.New().From("orders").Where("id").Greater(1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` >= @p0", _query.New().From("orders").WhereGreaterOrEqual("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` >= @p0", _query.New().From("orders").Where("id").GreaterOrEqual(1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where not `id` > @p0", _query.New().From("orders").WhereNotGreater("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where not `id` > @p0", _query.New().From("orders").Where("id").NotGreater(1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where not `id` >= @p0", _query.New().From("orders").WhereNotGreaterOrEqual("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where not `id` >= @p0", _query.New().From("orders").Where("id").NotGreaterOrEqual(1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` < @p0", _query.New().From("orders").WhereLess("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` < @p0", _query.New().From("orders").Where("id").Less(1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where not `id` < @p0", _query.New().From("orders").WhereNotLess("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where not `id` < @p0", _query.New().From("orders").Where("id").NotLess(1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` <= @p0", _query.New().From("orders").WhereLessOrEqual("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` <= @p0", _query.New().From("orders").Where("id").LessOrEqual(1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where not `id` <= @p0", _query.New().From("orders").WhereNotLessOrEqual("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where not `id` <= @p0", _query.New().From("orders").Where("id").NotLessOrEqual(1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` like %@p0%", _query.New().From("orders").WhereLike("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` like %@p0%", _query.New().From("orders").Where("id").Like(1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` not like %@p0%", _query.New().From("orders").WhereNotLike("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` not like %@p0%", _query.New().From("orders").Where("id").NotLike(1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` like @p0%", _query.New().From("orders").WhereStarts("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` like @p0%", _query.New().From("orders").Where("id").Starts(1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` not like @p0%", _query.New().From("orders").WhereNotStarts("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` not like @p0%", _query.New().From("orders").Where("id").NotStarts(1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` like %@p0", _query.New().From("orders").WhereEnds("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` like %@p0", _query.New().From("orders").Where("id").Ends(1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` not like %@p0", _query.New().From("orders").WhereNotEnds("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` not like %@p0", _query.New().From("orders").Where("id").NotEnds(1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` between @p0 and @p1", _query.New().From("orders").WhereBetween("id", 1, 3).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` between @p0 and @p1", _query.New().From("orders").Where("id").Between(1, 3).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` not between @p0 and @p1", _query.New().From("orders").WhereNotBetween("id", 1, 3).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` not between @p0 and @p1", _query.New().From("orders").Where("id").NotBetween(1, 3).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` is null", _query.New().From("orders").WhereNull("id").Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` is null", _query.New().From("orders").Where("id").Null().Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` is not null", _query.New().From("orders").WhereNotNull("id").Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` is not null", _query.New().From("orders").Where("id").NotNull().Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` in @p0", _query.New().From("orders").WhereIn("id", $"({string.Join(", ", new int[] { 1, 2, 3 })})").Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` in @p0", _query.New().From("orders").Where("id").In($"({string.Join(", ", new int[] { 1, 2, 3 })})").Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` not in @p0", _query.New().From("orders").WhereNotIn("id", $"({string.Join(", ", new int[] { 1, 2, 3 })})").Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` not in @p0", _query.New().From("orders").Where("id").NotIn($"({string.Join(", ", new int[] { 1, 2, 3 })})").Compile().CompiledSql);

            Assert.Equal("select * from `orders` where `id` = all (select `id` from `orders` where `total` > @p0)", _query.New().From("orders").WhereAll("id", "=", q => q.From("orders").Select("id").WhereGreater("total", 10)).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` = all (select `id` from `orders` where `total` > @p0)", _query.New().From("orders").Where("id").All("=", q => q.From("orders").Select("id").WhereGreater("total", 10)).Compile().CompiledSql);

            Assert.Equal("select * from `orders` where `id` = any (select `id` from `orders` where `total` > @p0)", _query.New().From("orders").WhereAny("id", "=", q => q.From("orders").Select("id").WhereGreater("total", 10)).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where `id` = any (select `id` from `orders` where `total` > @p0)", _query.New().From("orders").Where("id").Any("=", q => q.From("orders").Select("id").WhereGreater("total", 10)).Compile().CompiledSql);

            Assert.Equal("select * from `orders` where exists (select `id` from `orders` where `total` > @p0)", _query.New().From("orders").WhereExists(q => q.From("orders").Select("id").WhereGreater("total", 10)).Compile().CompiledSql);
            Assert.Equal("select * from `orders` where not exists (select `id` from `orders` where `total` > @p0)", _query.New().From("orders").WhereNotExists(q => q.From("orders").Select("id").WhereGreater("total", 10)).Compile().CompiledSql);
        }
    }
}
