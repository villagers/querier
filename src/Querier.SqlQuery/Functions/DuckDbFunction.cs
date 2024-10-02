namespace Querier.SqlQuery.Functions
{
    public class DuckDbFunction : BaseFunction, IFunction
    {
        public IFunction New()
        {
            return new MySqlFunction();
        }
        public IFunction Date(string column, string? columnAs = null)
        {
            AddLast($"datepart('year', @table.{NameParameter(column)})");
            if (!string.IsNullOrWhiteSpace(columnAs))
            {
                AddLast("as");
                AddLast(NameParameter(columnAs));
            }
            return this;
        }
        public IFunction Year(string column, string? columnAs = null)
        {
            AddLast($"year(@table.{NameParameter(column)})");
            if (!string.IsNullOrWhiteSpace(columnAs))
            {
                AddLast("as");
                AddLast(NameParameter(columnAs));
            }
            return this;
        }

        public IFunction Month(string column, string? columnAs = null)
        {
            AddLast($"month(@table.{NameParameter(column)})");
            if (!string.IsNullOrWhiteSpace(columnAs))
            {
                AddLast("as");
                AddLast(NameParameter(columnAs));
            }
            return this;
        }
        public IFunction Day(string column, string? columnAs = null)
        {
            AddLast($"day(@table.{NameParameter(column)})");
            if (!string.IsNullOrWhiteSpace(columnAs))
            {
                AddLast("as");
                AddLast(NameParameter(columnAs));
            }
            return this;
        }
        public IFunction Hour(string column, string? columnAs = null)
        {
            AddLast($"hour(@table.{NameParameter(column)})");
            if (!string.IsNullOrWhiteSpace(columnAs))
            {
                AddLast("as");
                AddLast(NameParameter(columnAs));
            }
            return this;
        }

        public IFunction Minute(string column, string? columnAs = null)
        {
            AddLast($"minute(@table.{NameParameter(column)})");
            if (!string.IsNullOrWhiteSpace(columnAs))
            {
                AddLast("as");
                AddLast(NameParameter(columnAs));
            }
            return this;
        }

        public IFunction Second(string column, string? columnAs = null)
        {
            AddLast($"second(@table.{NameParameter(column)})");
            if (!string.IsNullOrWhiteSpace(columnAs))
            {
                AddLast("as");
                AddLast(NameParameter(columnAs));
            }
            return this;
        }
    }
}
