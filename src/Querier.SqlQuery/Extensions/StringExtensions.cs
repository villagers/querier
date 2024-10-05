using System.Text.RegularExpressions;

namespace Querier.SqlQuery.Extensions
{
    public static class StringExtensions
    {
        public static string ReplaceExact(this string str, string oldValue, string newValue)
        {
            var escaped = Regex.Escape(oldValue);
            var pattern = $@"(?<!\w){Regex.Escape(escaped)}(?!\w)";
            return Regex.Replace(str, pattern, newValue);
        }
    }
}
