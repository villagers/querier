using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Extensions
{
    public static class DictionaryExtensions
    {
        public static Dictionary<TKey, TValue> Merge<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Dictionary<TKey, TValue> dictionaryToMerge)
        {
            foreach (var kvp in dictionaryToMerge)
            {
                if (!dictionary.ContainsKey(kvp.Key))
                {
                    dictionary[kvp.Key] = kvp.Value;
                }
            }

            return dictionary;
        }
    }
}
