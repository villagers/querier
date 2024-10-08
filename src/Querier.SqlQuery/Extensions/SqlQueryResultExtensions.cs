﻿using Querier.SqlQuery.Models;

namespace Querier.SqlQuery.Extensions
{
    public static class SqlQueryResultExtensions
    {
        public static SqlQueryResult Merge(this SqlQueryResult result, SqlQueryResult resultToMerge)
        {
            foreach (var item in resultToMerge.NameParameters.Keys.ToList())
            {
                resultToMerge.NameParameters.RenameKey(item, $"@temp_{item}");
                resultToMerge.Sql = resultToMerge.Sql.ReplaceExact(item, $"@temp_{item}");
            }
            foreach (var item in resultToMerge.SqlParameters.Keys.ToList())
            {
                resultToMerge.SqlParameters.RenameKey(item, $"@temp_{item}");
                resultToMerge.Sql = resultToMerge.Sql.ReplaceExact(item, $"@temp_{item}");
            }


            foreach (var nameParameter in resultToMerge.NameParameters)
            {
                var count = result.NameParameters.Count;

                var newKey = $"@n{count}";
                result.NameParameters.Add(newKey, nameParameter.Value);

                resultToMerge.Sql = resultToMerge.Sql.ReplaceExact(nameParameter.Key, newKey);
            }

            foreach (var sqlParameter in resultToMerge.SqlParameters)
            {
                var count = result.SqlParameters.Count;

                var newKey = $"@p{count}";
                result.SqlParameters.Add(newKey, sqlParameter.Value);

                resultToMerge.Sql = resultToMerge.Sql.ReplaceExact(sqlParameter.Key, newKey);
            }
            result.SqlTokenizer.AddToken(resultToMerge.Sql);

            result.Sql = result.SqlTokenizer.Build();
            return result;
        }
        public static SqlQueryResult Enumerate(this SqlQueryResult result)
        {
            result.NameParameters = result.NameParameters.Select((e, i) =>
            {
                result.Sql = result.Sql.ReplaceExact(e.Key, $"@n{i}");
                return new KeyValuePair<string, string>($"@n{i}", e.Value);
            }).ToDictionary();

            result.SqlParameters = result.SqlParameters.Select((e, i) =>
            {
                result.Sql = result.Sql.ReplaceExact(e.Key, $"@p{i}");
                return new KeyValuePair<string, object>($"@p{i}", e.Value);
            }).ToDictionary();

            return result;
        }
    }
}
