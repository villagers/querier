namespace Querier.SqlQuery.Tokenizers
{
    public class SqlTokenizer : LinkedList<string>
    {
        private SqlTokenizer _child;
        public readonly Dictionary<string, string> NameParameters;

        public SqlTokenizer(string? token = null)
        {
            AddToken(token);
            NameParameters = new Dictionary<string, string>();
        }

        public SqlTokenizer AddToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return this;

            AddLast(token);
            return this;
        }
        public SqlTokenizer AddToken(Func<SqlTokenizer, SqlTokenizer> tokenizer, string separator = " ")
        {
            _child = new SqlTokenizer();
            var child = tokenizer.Invoke(_child);
            AddToken(child.Build(separator));
            return this;
        }

        public void AddAfter(string node, string newNode)
        {
            var nodeIn = Find(node);
            if (nodeIn == null)
            {
                throw new InvalidOperationException($"Node '{node}' does not exists");
            }
            AddAfter(nodeIn, new LinkedListNode<string>(newNode));
        }


        public string Build(string separator = " ")
        {
            return string.Join(separator, this);
        }
    }
}
