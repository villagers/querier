using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Querier.SqlQuery.Functions
{
    public class MySqlFunction : BaseFunction, IFunction
    {
        public IFunction New()
        {
            return new MySqlFunction();
        }

        public IFunction Date(string column)
        {
            AddLast($"date({NameParameter(column)})");
            return this;
        }
        public IFunction Year(string column)
        {
            AddLast($"year({NameParameter(column)})");
            return this;
        }

        public IFunction Month(string column)
        {
            AddLast($"month({NameParameter(column)})");
            return this;
        }
        public IFunction Day(string column)
        {
            AddLast($"day({NameParameter(column)})");
            return this;
        }
        public IFunction Hour(string column)
        {
            AddLast($"hour({NameParameter(column)})");
            return this;
        }

        public IFunction Minute(string column)
        {
            AddLast($"minute({NameParameter(column)})");
            return this;
        }

        public IFunction Second(string column)
        {
            AddLast($"second({NameParameter(column)})");
            return this;
        }
    }
}
