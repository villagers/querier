using Querier.SqlQuery.Models;
using Querier.SqlQuery.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery
{
    public abstract class BaseAbstractQuery
    {
        public BaseAbstractQuery() { }
        public abstract SqlQueryResult CompileSql(SqlQueryResult result);
        public abstract SqlTokenizer CompileTokens(SqlQueryResult result);
        public abstract Dictionary<string, object> CompileSqlParameters(SqlQueryResult result);
        public abstract Dictionary<string, string> CompileNameParameters(SqlQueryResult result);

    }
}
