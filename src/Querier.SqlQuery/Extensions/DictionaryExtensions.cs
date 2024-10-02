namespace Querier.SqlQuery.Extensions
{
    public static class DictionaryExtensions
    {
        public static Dictionary<string, TValue> Merge<TValue>(this Dictionary<string, TValue> dictionary, Dictionary<string, TValue> dictionaryToMerge, string prefix = "@n")
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

        public static Dictionary<string, TValue> CopyTo<TValue>(this Dictionary<string, TValue> dictionary, Dictionary<string, TValue> dictionaryToCopy, string prefix = "@n")
        {
            var index = dictionaryToCopy.Count;

            foreach (var item in dictionary)
            {
                dictionaryToCopy.Add($"{prefix}{index++}", item.Value);
            }

            return dictionary;
        }

        public static void RenameKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey fromKey, TKey toKey)
        {
            TValue value = dictionary[fromKey];
            dictionary.Remove(fromKey);
            dictionary[toKey] = value;
        }
    }
}
