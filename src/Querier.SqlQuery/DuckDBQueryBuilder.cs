﻿using Querier.SqlQuery.Functions;
using Querier.SqlQuery.Models;

namespace Querier.SqlQuery
{
    public class DuckDBQueryBuilder : BaseQuery<IDuckDBQueryBuilder>, IDuckDBQueryBuilder
    {
        protected override string NameParameterOpening => "";
        protected override string NameParameterClosing => "";
        protected override string NameParameterPlaceholder { get; set; } = "@n";
        protected override string SqlParameterPlaceholder { get; set; } = "$p";
        public DuckDBQueryBuilder(IFunction functionFactory) : base(functionFactory)
        {
        }

        public override IDuckDBQueryBuilder New()
        {
            return new DuckDBQueryBuilder(_functionFactory);
        }

        public override IDuckDBQueryBuilder CompileFull(SqlQueryResult result)
        {
            result.SqlParameters = result.SqlParameters
                .Select((e, i) =>
                {
                    return new KeyValuePair<string, object>($"{SqlParameterPlaceholder.Substring(1)}{i}", e.Value);
                }).ToDictionary();

            return this;
        }

    }
}
