using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Models;

namespace Querier.SqlQuery.Operators
{
    public abstract class AbstractOperator : ISqlQueryCompile<SqlQueryResult>, ICloneable
    {
        public ISqlColumn? Column { get; set; }

        protected bool HasAnd = false;
        protected bool HasOr = false;
        protected bool HasNot = false;

        protected string NotOperator { get; set; } = string.Empty;
        protected string AndOrOperator { get; set; } = string.Empty;

        public abstract SqlQueryResult Compile(ISqlTable table);

        public AbstractOperator And(bool flag = true)
        {
            HasAnd = flag;
            if (flag)
            {
                AndOrOperator = "and";
            }
            return this;
        }
        public AbstractOperator Or(bool flag = true)
        {
            HasOr = flag;
            if (flag)
            {
                AndOrOperator = "or";
            }
            return this;
        }
        public AbstractOperator Not(bool flag = true)
        {
            HasNot = flag;
            if (flag)
            {
                NotOperator = "not";
            }
            return this;
        }

        public bool IsOperatorType<T>()
        {
            var thisType = GetType().BaseType;
            var type = typeof(T);
            return thisType == type;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
