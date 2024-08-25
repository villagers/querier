using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Extensions
{
    public static class DictionaryExtensions
    {

        public static Dictionary<string, string> Merge(this Dictionary<string, string> dictionary, string suffix, Dictionary<string, string> dictionaryToMerge)
        {
            var index = 0;

            var result = new Dictionary<string, string>();

            foreach (var item in dictionary)
            {
                result.Add($"{suffix}{index++}", item.Value);
            }

            foreach (var item in dictionaryToMerge)
            {
                result.Add($"{suffix}{index++}", item.Value);
            }


            return result;
        }
    }
}
