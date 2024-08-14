using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Models;
using Querier.SqlQuery.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery
{
    public class SqlQuery : Query<SqlQuery>, IQuery<SqlQuery>, ISqlQuery
    {

        public override SqlQueryResult Compile()
        {

            SqlParameters = new Dictionary<string, object>();
            NameParameters = new Dictionary<string, string>();

            var result = new SqlQueryResult();
            var queryTz = new SqlTokenizer();

            var selectQuery = CreateSelect();
            var tableQuery = CreateTable();
            var whereQuery = CreateWhere();
            var groupByQuery = CreateGroupBy();
            var orderByQuery = CreateOrderBy();

            queryTz.AddToken(selectQuery.Sql);
            queryTz.AddToken(tableQuery.Sql);
            queryTz.AddToken(whereQuery.Sql);
            queryTz.AddToken(groupByQuery.Sql);
            queryTz.AddToken(orderByQuery.Sql);
            result.Sql = queryTz.Build(" ");


            foreach (var param in selectQuery.SqlParameters)
            {
                SqlParameters.TryAdd(param.Key, param.Value);
            }
            foreach (var param in selectQuery.NameParameters)
            {
                NameParameters.TryAdd(param.Key, param.Value);
            }

            foreach (var param in tableQuery.SqlParameters)
            {
                SqlParameters.TryAdd(param.Key, param.Value);
            }
            foreach (var param in tableQuery.NameParameters)
            {
                NameParameters.TryAdd(param.Key, param.Value);
            }

            foreach (var param in whereQuery.SqlParameters)
            {
                SqlParameters.TryAdd(param.Key, param.Value);
            }
            foreach (var param in whereQuery.NameParameters)
            {
                NameParameters.TryAdd(param.Key, param.Value);
            }
            foreach (var param in groupByQuery.SqlParameters)
            {
                SqlParameters.TryAdd(param.Key, param.Value);
            }

            foreach (var param in groupByQuery.NameParameters)
            {
                NameParameters.TryAdd(param.Key, param.Value);
            }

            foreach (var param in orderByQuery.SqlParameters)
            {
                SqlParameters.TryAdd(param.Key, param.Value);
            }
            foreach (var param in orderByQuery.NameParameters)
            {
                NameParameters.TryAdd(param.Key, param.Value);
            }

            result.NameParameters = NameParameters;
            result.SqlParameters = SqlParameters;
            result.CompiledSql = new string(result.Sql);

            result.NameParameters = result.NameParameters.Select((e, i) =>
            {
                result.Sql = result.Sql.Replace(e.Key, $"@n{i}");
                return new KeyValuePair<string, string>($"@n{i}", e.Value);
            }).ToDictionary();

            foreach (var param in result.NameParameters)
            {
                result.CompiledSql = result.CompiledSql.Replace(param.Key, param.Value);
            }

            foreach (var param in SqlParameters)
            {
                result.CompiledSql = result.CompiledSql.Replace(param.Key, param.Value.ToString());
            }

            return result;
        }
    }
}
