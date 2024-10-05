using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Models;

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

        public SqlQueryResult Compile(ISqlTable table)
        {
            NameParameters.Add("@table", table.TableOrAlias);

            var result = new SqlQueryResult()
            {
                Sql = string.Join(" ", this),
                NameParameters = NameParameters
            };
            return result;
        }
    }
}
