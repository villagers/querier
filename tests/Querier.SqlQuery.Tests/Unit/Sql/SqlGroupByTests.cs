﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Tests.Unit.Sql
{
    public class SqlGroupByTests
    {
        private ISqlQuery _query;

        public SqlGroupByTests()
        {
            _query = new SqlQuery();
        }

        [Fact]
        public void GroupBy()
        {
            Assert.Equal("select * from orders group by product", _query.New().From("orders").GroupBy("product").Compile().CompiledSql);
            Assert.Equal("select * from orders group by product, category", _query.New().From("orders").GroupBy("product").GroupBy("category").Compile().CompiledSql);
        }
    }
}