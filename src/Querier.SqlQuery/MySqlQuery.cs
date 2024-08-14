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
    public class MySqlQuery : Query<MySqlQuery>, IQuery<MySqlQuery>, IMySqlQuery
    {
        private int? _limit;

        public MySqlQuery()
        {
        }

        public IMySqlQuery Limit(int limit)
        {
            _limit = limit;
            return this;
        }

        public override SqlQueryResult Compile()
        {
            var result = new SqlQueryResult();
            var queryTz = new SqlTokenizer();

            var selectQuery = CreateSelect();
            if (_limit.HasValue)
            {
                selectQuery.SqlTokenizer.AddAfter("select", $"top({_limit})");
                selectQuery.Sql = selectQuery.SqlTokenizer.Build();
            }
            var tableQuery = CreateTable();
            var whereQuery = CreateWhere();
            var groupByQuery = CreateGroupBy();
            var orderByQuery = CreateOrderBy();

            queryTz.AddToken(selectQuery.Sql);
            foreach (var param in selectQuery.SqlParameters)
            {
                result.SqlParameters.Add(param.Key, param.Value);
            }
            foreach (var param in selectQuery.NameParameters)
            {
                result.NameParameters.Add(param.Key, param.Value);
            }

            queryTz.AddToken(tableQuery.Sql);
            foreach (var param in tableQuery.SqlParameters)
            {
                result.SqlParameters.Add(param.Key, param.Value);
            }
            foreach (var param in tableQuery.NameParameters)
            {
                result.NameParameters.Add(param.Key, param.Value);
            }

            queryTz.AddToken(whereQuery.Sql);
            foreach (var param in whereQuery.SqlParameters)
            {
                result.SqlParameters.Add(param.Key, param.Value);
            }

            queryTz.AddToken(groupByQuery.Sql);
            foreach (var param in groupByQuery.SqlParameters)
            {
                result.SqlParameters.Add(param.Key, param.Value);
            }

            queryTz.AddToken(orderByQuery.Sql);
            foreach (var param in orderByQuery.SqlParameters)
            {
                result.SqlParameters.Add(param.Key, param.Value);
            }

            result.Sql = queryTz.Build(" ");

            return result;
        }
    }
}
