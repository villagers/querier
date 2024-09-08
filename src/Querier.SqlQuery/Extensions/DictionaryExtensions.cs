using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Extensions
{
    public static class DictionaryExtensions
    {
        public static Dictionary<string, TValue> Merge<TValue>(this Dictionary<string, TValue> dictionary, Dictionary<string, TValue> dictionaryToMerge, string prefix = "@name")
        {
            var index = 0;
            var result = new Dictionary<string, TValue>();

            foreach (var item in dictionary)
            {
                result.Add($"{prefix}{index++}", item.Value);
            }

            if (dictionaryToMerge != null)
            {
                foreach (var item in dictionaryToMerge)
                {
                    result.Add($"{prefix}{index++}", item.Value);
                }
            }

            return result;
        }
    }
}
