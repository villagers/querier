using Querier.SqlQuery.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.MySql.Tests.Unit
{
    public class UnionTests
    {
        private IMySqlQuery _query;

        public UnionTests()
        {
            _query = new MySqlQuery(new MySqlFunction());
        }

        [Fact]
        public void Union()
        {
            Assert.Equal("select `product_id` from `orders` union select `product_id` from `orders`",
                _query.New().From("orders").Select("product_id")
                .Union(_query.New().From("orders").Select("product_id")).Compile().CompiledSql);

            Assert.Equal("select `product_id` from `orders` union select `product_id` from `orders` where `id` = @p0",
                _query.New().From("orders").Select("product_id")
                .Union(_query.New().From("orders").Select("product_id").WhereEqual("id", 10)).Compile().CompiledSql);

            Assert.Equal("select `product_id` from `orders` where `id` < @p0 union select `product_id` from `orders` where `id` = @p1",
                _query.New().From("orders").Select("product_id").WhereLess("id", 10)
                .Union(_query.New().From("orders").Select("product_id").WhereEqual("id", 10)).Compile().CompiledSql);

            Assert.Equal("select `product_id` from `orders` where `id` < @p0 union select `product_id` from `orders` where `id` = @p1 order by `total` asc",
                _query.New().From("orders").Select("product_id").WhereLess("id", 10)
                .Union(_query.New().From("orders").Select("product_id").WhereEqual("id", 10))
                .OrderBy("total").Compile().CompiledSql);

            Assert.Equal("select `product_id` from `orders` where `id` < @p0 union select `product_id` from `orders` where `id` = @p1 union select `product_id` from `orders` where `id` > @p2 or `id` > @p3 order by `total` asc",
                _query.New().From("orders").Select("product_id").WhereLess("id", 10)
                .Union(_query.New().From("orders").Select("product_id").WhereEqual("id", 10))
                .Union(_query.New().From("orders").Select("product_id").WhereGreater("id", 20).Or(40))
                .OrderBy("total").Compile().CompiledSql);
        }

        [Fact]
        public void UnionAll()
        {
            Assert.Equal("select `product_id` from `orders` union all select `product_id` from `orders`",
                _query.New().From("orders").Select("product_id")
                .UnionAll(_query.New().From("orders").Select("product_id")).Compile().CompiledSql);

            Assert.Equal("select `product_id` from `orders` union all select `product_id` from `orders` where `id` = @p0",
                _query.New().From("orders").Select("product_id")
                .UnionAll(_query.New().From("orders").Select("product_id").WhereEqual("id", 10)).Compile().CompiledSql);

            Assert.Equal("select `product_id` from `orders` where `id` < @p0 union all select `product_id` from `orders` where `id` = @p1",
                _query.New().From("orders").Select("product_id").WhereLess("id", 10)
                .UnionAll(_query.New().From("orders").Select("product_id").WhereEqual("id", 10)).Compile().CompiledSql);

            Assert.Equal("select `product_id` from `orders` where `id` < @p0 union all select `product_id` from `orders` where `id` = @p1 order by `total` asc",
                _query.New().From("orders").Select("product_id").WhereLess("id", 10)
                .UnionAll(_query.New().From("orders").Select("product_id").WhereEqual("id", 10)).OrderBy("total").Compile().CompiledSql);

            Assert.Equal("select `product_id` from `orders` where `id` < @p0 union all select `product_id` from `orders` where `id` = @p1 union all select `product_id` from `orders` where `id` > @p2 or `id` > @p3 order by `total` asc",
                _query.New().From("orders").Select("product_id").WhereLess("id", 10)
                .UnionAll(_query.New().From("orders").Select("product_id").WhereEqual("id", 10))
                .UnionAll(_query.New().From("orders").Select("product_id").WhereGreater("id", 20).Or(40))
                .OrderBy("total").Compile().CompiledSql);
        }
    }
}
