using Querier.SqlQuery.Functions;

namespace Querier.SqlQuery.MySql.Tests.Unit
{
    public class SqlWhereTests
    {
        private IMySqlQueryBuilder _query;

        public SqlWhereTests()
        {
            _query = new MySqlQueryBuilder(new MySqlFunction());
        }

        [Fact]
        public void Where()
        {
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` = @p0", _query.New().Select().From("orders").Where("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` = @p0", _query.New().Select().From("orders").WhereEqual("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` = @p0", _query.New().Select().From("orders").Where("id").Equal(1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` != @p0", _query.New().Select().From("orders").WhereNotEqual("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` != @p0", _query.New().Select().From("orders").Where("id").NotEqual(1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` > @p0", _query.New().Select().From("orders").WhereGreater("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` > @p0", _query.New().Select().From("orders").Where("id").Greater(1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` >= @p0", _query.New().Select().From("orders").WhereGreaterOrEqual("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` >= @p0", _query.New().Select().From("orders").Where("id").GreaterOrEqual(1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where not `orders`.`id` > @p0", _query.New().Select().From("orders").WhereNotGreater("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where not `orders`.`id` > @p0", _query.New().Select().From("orders").Where("id").NotGreater(1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where not `orders`.`id` >= @p0", _query.New().Select().From("orders").WhereNotGreaterOrEqual("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where not `orders`.`id` >= @p0", _query.New().Select().From("orders").Where("id").NotGreaterOrEqual(1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` < @p0", _query.New().Select().From("orders").WhereLess("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` < @p0", _query.New().Select().From("orders").Where("id").Less(1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where not `orders`.`id` < @p0", _query.New().Select().From("orders").WhereNotLess("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where not `orders`.`id` < @p0", _query.New().Select().From("orders").Where("id").NotLess(1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` <= @p0", _query.New().Select().From("orders").WhereLessOrEqual("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` <= @p0", _query.New().Select().From("orders").Where("id").LessOrEqual(1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where not `orders`.`id` <= @p0", _query.New().Select().From("orders").WhereNotLessOrEqual("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where not `orders`.`id` <= @p0", _query.New().Select().From("orders").Where("id").NotLessOrEqual(1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` like %@p0%", _query.New().Select().From("orders").WhereLike("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` like %@p0%", _query.New().Select().From("orders").Where("id").Like(1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` not like %@p0%", _query.New().Select().From("orders").WhereNotLike("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` not like %@p0%", _query.New().Select().From("orders").Where("id").NotLike(1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` like @p0%", _query.New().Select().From("orders").WhereStarts("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` like @p0%", _query.New().Select().From("orders").Where("id").Starts(1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` not like @p0%", _query.New().Select().From("orders").WhereNotStarts("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` not like @p0%", _query.New().Select().From("orders").Where("id").NotStarts(1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` like %@p0", _query.New().Select().From("orders").WhereEnds("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` like %@p0", _query.New().Select().From("orders").Where("id").Ends(1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` not like %@p0", _query.New().Select().From("orders").WhereNotEnds("id", 1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` not like %@p0", _query.New().Select().From("orders").Where("id").NotEnds(1).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` between @p0 and @p1", _query.New().Select().From("orders").WhereBetween("id", 1, 3).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` between @p0 and @p1", _query.New().Select().From("orders").Where("id").Between(1, 3).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` not between @p0 and @p1", _query.New().Select().From("orders").WhereNotBetween("id", 1, 3).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` not between @p0 and @p1", _query.New().Select().From("orders").Where("id").NotBetween(1, 3).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` is null", _query.New().Select().From("orders").WhereNull("id").Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` is null", _query.New().Select().From("orders").Where("id").Null().Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` is not null", _query.New().Select().From("orders").WhereNotNull("id").Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` is not null", _query.New().Select().From("orders").Where("id").NotNull().Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` in @p0", _query.New().Select().From("orders").WhereIn("id", new int[] { 1, 2, 3 }).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` in @p0", _query.New().Select().From("orders").Where("id").In(new int[] { 1, 2, 3 }).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` not in @p0", _query.New().Select().From("orders").WhereNotIn("id", new int[] { 1, 2, 3 }).Compile().CompiledSql);
            Assert.Equal("select * from `orders` as `orders` where `orders`.`id` not in @p0", _query.New().Select().From("orders").Where("id").NotIn(new int[] { 1, 2, 3 }).Compile().CompiledSql);
        }
    }
}
