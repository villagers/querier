﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Tests.Unit.Sql
{
    public class OrderByTests
    {
        private ISqlQuery _query;

        public OrderByTests()
        {
            _query = new SqlQuery();
        }

        [Fact]
        public void OrderBy()
        {
            Assert.Equal("select * from orders order by product_id asc", _query.New().From("orders").OrderBy("product_id").Compile().CompiledSql);
            Assert.Equal("select * from orders order by product_id asc, category_id desc", _query.New().From("orders").OrderBy("product_id").OrderBy("category_id", "desc").Compile().CompiledSql);
        }
    }
}