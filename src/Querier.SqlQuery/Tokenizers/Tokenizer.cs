using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Tokenizers
{
    public class Tokenizer : LinkedList<string>
    {
        private Tokenizer _child;
        public readonly Dictionary<string, string> NameParameters;

        public Tokenizer(string? token = null)
        {
            AddToken(token);
            NameParameters = new Dictionary<string, string>();
        }

        public Tokenizer AddToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return this;

            AddLast(token);
            return this;
        }
        public Tokenizer AddToken(Func<Tokenizer, Tokenizer> tokenizer, string separator = " ")
        {
            _child = new Tokenizer();
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
