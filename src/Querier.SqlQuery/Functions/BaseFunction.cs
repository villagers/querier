using Querier.SqlQuery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Functions
{
    public class BaseFunction : LinkedList<string>
    {
        public Dictionary<string, string> NameParameters { get; set; }

        public BaseFunction()
        {
            NameParameters = new Dictionary<string, string>();
        }

        protected string NameParameter(string parameter)
        {
            var count = NameParameters.Count;
            var newParameter = $"@f{count}";
            NameParameters.Add(newParameter, parameter);

            return newParameter;
        }

        public SqlFunctionResult Compile()
        {
            var result = new SqlFunctionResult()
            {
                Sql = string.Join(" ", this),
                NameParameters = NameParameters
            };
            return result;
        }
    }
}
