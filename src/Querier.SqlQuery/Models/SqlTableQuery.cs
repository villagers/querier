﻿using Querier.SqlQuery.Interfaces;
using Querier.SqlQuery.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.SqlQuery.Models
{
    public class SqlTableQuery<TQuery> :  SqlTable<TQuery> where TQuery : IBaseQuery<TQuery>
    {
        public override SqlQueryResult Compile()
        {
            var result = Query?.Compile();
            var queryTz = new SqlTokenizer().AddToken("(").AddToken(result?.Sql).AddToken(")").Build("");
            var tableTz = new SqlTokenizer().AddToken(queryTz);

            if (!string.IsNullOrEmpty(TableAs))
            {
                result.NameParameters.Add("@as", TableAs);
                tableTz.AddToken("as").AddToken("@as");
            }

            result.Sql = tableTz.Build(" ");
            result.NameParameters = result.NameParameters.Select((e, i) =>
            {
                result.Sql = result.Sql.Replace(e.Key, $"@name{i}");
                return new KeyValuePair<string, string>($"@name{i}", e.Value);
            }).ToDictionary();
            return result;
        }
    }
}