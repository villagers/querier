namespace Querier.Extensions
{
    public static class EnumerableExtensions
    {
        public static Dictionary<string, object> Flatten(this IEnumerable<Dictionary<string, object>> enumerable, string key, string value)
        {
            return enumerable.ToDictionary(item => (string)item[key], item => item[value]);
        }
    }
}
