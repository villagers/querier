using Querier.SqlQuery.Functions;
using Querier.SqlQuery.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Models
{
    public class SqlColumn : ISqlQueryCompile<SqlFunctionResult>
    {
        public string? Column {  get; set; }
        public IFunction? Function { get; set; }

        public SqlFunctionResult Compile()
        {
            return Function?.Compile() ?? new SqlFunctionResult() { Sql = Column, NameParameters = new Dictionary<string, string>() { { "@column", Column } } };
        }
    }
}
