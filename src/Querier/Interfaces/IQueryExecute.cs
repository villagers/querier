using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Interfaces
{
    public interface IQueryExecute
    {
        QueryResult Execute();

        IEnumerable<Dictionary<string, object>>? Get();
        IEnumerable<T>? GetValues<T>(string property);
        IEnumerable<object>? GetValues(string property);
        


        T? GetScalar<T>();
        Task<T?> GetScalarAsync<T>();
        object? GetScalar();
        Task<object?> GetScalarAsync();

        IDictionary<string, object>? GetSingle();
        Task<IDictionary<string, object>?> GetSingleAsync();
        T? GetSingleValue<T>(string? property = null);
        Task<T?> GetSingleValueAsync<T>(string? property = null);
        object? GetSingleValue(string? property = null);
        Task<object?> GetSingleValueAsync(string? property = null);

        IDictionary<string, object>? GetFirst();
        Task<IDictionary<string, object>?> GetFirstAsync();
        T? GetFirstValue<T>(string? property = null);
        Task<T?> GetFirstValueAsync<T>(string? property = null);
        object? GetFirstValue(string? property = null);
        Task<object?> GetFirstValueAsync(string? property = null); 
    }
}
