using Querier.SqlQuery.Functions;

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
        
    }
}
