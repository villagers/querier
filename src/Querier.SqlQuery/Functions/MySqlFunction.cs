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

        public IFunction Date(string column, string? columnAs = null)
        {
            AddLast($"date({NameParameter(column)})");
            if (!string.IsNullOrWhiteSpace(columnAs))
            {
                AddLast("as");
                AddLast(NameParameter(columnAs));
            }
            return this;
        }
        public IFunction Year(string column, string? columnAs = null)
        {
            AddLast($"year({NameParameter(column)})");
            if (!string.IsNullOrWhiteSpace(columnAs))
            {
                AddLast("as");
                AddLast(NameParameter(columnAs));
            }
            return this;
        }

        public IFunction Month(string column, string? columnAs = null)
        {
            AddLast($"month({NameParameter(column)})");
            if (!string.IsNullOrWhiteSpace(columnAs))
            {
                AddLast("as");
                AddLast(NameParameter(columnAs));
            }
            return this;
        }
        public IFunction Day(string column, string? columnAs = null)
        {
            AddLast($"day({NameParameter(column)})");
            if (!string.IsNullOrWhiteSpace(columnAs))
            {
                AddLast("as");
                AddLast(NameParameter(columnAs));
            }
            return this;
        }
        public IFunction Hour(string column, string? columnAs = null)
        {
            AddLast($"hour({NameParameter(column)})");
            if (!string.IsNullOrWhiteSpace(columnAs))
            {
                AddLast("as");
                AddLast(NameParameter(columnAs));
            }
            return this;
        }

        public IFunction Minute(string column, string? columnAs = null)
        {
            AddLast($"minute({NameParameter(column)})");
            if (!string.IsNullOrWhiteSpace(columnAs))
            {
                AddLast("as");
                AddLast(NameParameter(columnAs));
            }
            return this;
        }

        public IFunction Second(string column, string? columnAs = null)
        {
            AddLast($"second({NameParameter(column)})");
            if (!string.IsNullOrWhiteSpace(columnAs))
            {
                AddLast("as");
                AddLast(NameParameter(columnAs));
            }
            return this;
        }
    }
}
