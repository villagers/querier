using Querier.SqlQuery.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.MySql.Tests.Unit
{
    public class SqlUnionTests
    {
        private IMySqlQuery _query;

        public SqlUnionTests()
        {
            _query = new MySqlQuery(new MySqlFunction());
        }

        [Fact]
        public void Union()
        {
            const string union1 = "select `orders`.`product_id` from `orders` as `orders` union select `orders`.`product_id` from `orders` as `orders`";
            const string union2 = "select `orders`.`product_id` from `orders` as `orders` union select `orders`.`product_id` from `orders` as `orders` where `orders`.`id` = @p0";
            const string union3 = "select `orders`.`product_id` from `orders` as `orders` where `orders`.`id` < @p0 union select `orders`.`product_id` from `orders` as `orders` where `orders`.`id` = @p1";
            const string union4 = "select `orders`.`product_id` from `orders` as `orders` where `orders`.`id` < @p0 union select `orders`.`product_id` from `orders` as `orders` where `orders`.`id` = @p1 order by `orders`.`total` asc";
            const string union5 = "select `orders`.`product_id` from `orders` as `orders` where `orders`.`id` < @p0 union select `orders`.`product_id` from `orders` as `orders` where `orders`.`id` = @p1 union select `orders`.`product_id` from `orders` as `orders` where `orders`.`id` > @p2 or `orders`.`id` > @p3 order by `orders`.`total` asc";

            Assert.Equal(union1,
                _query.New().From("orders").Select("product_id")
                .Union(_query.New().From("orders").Select("product_id")).Compile().CompiledSql);
            Assert.Equal(union2,
                _query.New().From("orders").Select("product_id")
                .Union(_query.New().From("orders").Select("product_id").WhereEqual("id", 10)).Compile().CompiledSql);
            Assert.Equal(union3,
                _query.New().From("orders").Select("product_id").WhereLess("id", 10)
                .Union(_query.New().From("orders").Select("product_id").WhereEqual("id", 10)).Compile().CompiledSql);
            Assert.Equal(union4,
                _query.New().From("orders").Select("product_id").WhereLess("id", 10)
                .Union(_query.New().From("orders").Select("product_id").WhereEqual("id", 10))
                .OrderBy("total").Compile().CompiledSql);
            Assert.Equal(union5,
                _query.New().From("orders").Select("product_id").WhereLess("id", 10)
                .Union(_query.New().From("orders").Select("product_id").WhereEqual("id", 10))
                .Union(_query.New().From("orders").Select("product_id").WhereGreater("id", 20).Or(40))
                .OrderBy("total").Compile().CompiledSql);
        }

        [Fact]
        public void UnionAll()
        {
            const string union1 = "select `orders`.`product_id` from `orders` as `orders` union all select `orders`.`product_id` from `orders` as `orders`";
            const string union2 = "select `orders`.`product_id` from `orders` as `orders` union all select `orders`.`product_id` from `orders` as `orders` where `orders`.`id` = @p0";
            const string union3 = "select `orders`.`product_id` from `orders` as `orders` where `orders`.`id` < @p0 union all select `orders`.`product_id` from `orders` as `orders` where `orders`.`id` = @p1";
            const string union4 = "select `orders`.`product_id` from `orders` as `orders` where `orders`.`id` < @p0 union all select `orders`.`product_id` from `orders` as `orders` where `orders`.`id` = @p1 order by `orders`.`total` asc";
            const string union5 = "select `orders`.`product_id` from `orders` as `orders` where `orders`.`id` < @p0 union all select `orders`.`product_id` from `orders` as `orders` where `orders`.`id` = @p1 union all select `orders`.`product_id` from `orders` as `orders` where `orders`.`id` > @p2 or `orders`.`id` > @p3 order by `orders`.`total` asc";


            Assert.Equal(union1,
                _query.New().From("orders").Select("product_id")
                .UnionAll(_query.New().From("orders").Select("product_id")).Compile().CompiledSql);
            Assert.Equal(union2,
                _query.New().From("orders").Select("product_id")
                .UnionAll(_query.New().From("orders").Select("product_id").WhereEqual("id", 10)).Compile().CompiledSql);
            Assert.Equal(union3,
                _query.New().From("orders").Select("product_id").WhereLess("id", 10)
                .UnionAll(_query.New().From("orders").Select("product_id").WhereEqual("id", 10)).Compile().CompiledSql);
            Assert.Equal(union4,
                _query.New().From("orders").Select("product_id").WhereLess("id", 10)
                .UnionAll(_query.New().From("orders").Select("product_id").WhereEqual("id", 10)).OrderBy("total").Compile().CompiledSql);
            Assert.Equal(union5,
                _query.New().From("orders").Select("product_id").WhereLess("id", 10)
                .UnionAll(_query.New().From("orders").Select("product_id").WhereEqual("id", 10))
                .UnionAll(_query.New().From("orders").Select("product_id").WhereGreater("id", 20).Or(40))
                .OrderBy("total").Compile().CompiledSql);
        }
    }
}
