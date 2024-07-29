using Querier.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Querier
{
    internal static class QueryHelper
    {

        internal static QueryProperty? GetMeasureProperty<TType>(string queryKey)
        {
            var type = typeof(TType);


            var result = type.GetProperties()
                .Where(e => e.IsDefined(typeof(QueryMeasureAttribute), true))
                .Where(e => e.Name == queryKey)
                .Select(e => e.GetCustomAttribute<QueryMeasureAttribute>())
                .Select(e => new QueryProperty()
                {
                    Key = queryKey,
                    DisplayName = e.DisplayName,
                })
                .FirstOrDefault();

            return result;
        }
        internal static QueryProperty? GetDimensionProperty<TType>(string queryKey)
        {
            var type = typeof(TType);

            var result = type.GetProperties()
                .Where(e => e.IsDefined(typeof(QueryDimensionAttribute), true))
                .Where(e => e.Name == queryKey)
                .Select(e => e.GetCustomAttribute<QueryDimensionAttribute>())
                .Select(e => new QueryProperty()
                {
                    Key = queryKey,
                    DisplayName = e.DisplayName,
                })
                .FirstOrDefault();

            return result;
        }
        internal static QueryProperty? GetTimeDimensionProperty<TType>(string queryKey)
        {
            var type = typeof(TType);

            var result = type.GetProperties()
                .Where(e => e.IsDefined(typeof(QueryTimeDimensionAttribute), true))
                .Where(e => e.Name == queryKey)
                .Select(e => e.GetCustomAttribute<QueryTimeDimensionAttribute>())
                .Select(e => new QueryProperty()
                {
                    Key = queryKey,
                    DisplayName = e.DisplayName,
                })
                .FirstOrDefault();

            return result;
        }

        internal static List<QueryProperty> ListMeasureProperties<T>()
        {
            return ListMeasureProperties(typeof(T));
        }
        internal static List<QueryProperty> ListMeasureProperties(string queryKey)
        {
            var type = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(e => e.GetTypes())
                .Where(e => e.IsDefined(typeof(QueryAttribute), true))
                .FirstOrDefault(e => e.GetCustomAttribute<QueryAttribute>()?.Key == queryKey);

            return ListMeasureProperties(type);
        }
        internal static List<QueryProperty> ListMeasureProperties(Type type)
        {
            var flags = BindingFlags.Public | BindingFlags.Instance;

            var annotatedProperties = type.GetProperties(flags)
                .Where(e => e.IsDefined(typeof(QueryMeasureAttribute), true))
                .Select(e => new QueryProperty()
                {
                    Key = e.GetCustomAttribute<QueryMeasureAttribute>()?.Key ?? e.Name,
                    DisplayName = e.GetCustomAttribute<QueryMeasureAttribute>()?.DisplayName
                }).ToList();


            if (annotatedProperties.Count != 0)
            {
                return annotatedProperties;
            }

            var result = new List<QueryProperty>();
            var properties = type.GetProperties(flags);
            foreach (var property in properties)
            {
                var isMeasureType = IsMeasureType(property.PropertyType);
                if (!isMeasureType) continue;
                result.Add(new QueryProperty()
                {
                    Key = property.Name
                });
            }
            return result;
        }


        internal static List<QueryProperty> ListDimensionProperties<T>()
        {
            return ListDimensionProperties(typeof(T));
        }
        internal static List<QueryProperty> ListDimensionProperties(string queryKey)
        {
            var type = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(e => e.GetTypes())
                .Where(e => e.IsDefined(typeof(QueryAttribute), true))
                .FirstOrDefault(e => e.GetCustomAttribute<QueryAttribute>()?.Key == queryKey);

            return ListDimensionProperties(type);
        }
        internal static List<QueryProperty> ListDimensionProperties(Type type)
        {
            var flags = BindingFlags.Public | BindingFlags.Instance;

            var annotatedProperties = type.GetProperties(flags)
                .Where(e => e.IsDefined(typeof(QueryDimensionAttribute), true))
                .Select(e => new QueryProperty()
                {
                    Key = e.GetCustomAttribute<QueryDimensionAttribute>()?.Key ?? e.Name,
                    DisplayName = e.GetCustomAttribute<QueryDimensionAttribute>()?.DisplayName
                }).ToList();
            if (annotatedProperties.Count != 0)
            {
                return annotatedProperties;
            }

            var result = new List<QueryProperty>();
            var properties = type.GetProperties(flags);
            foreach (var property in properties)
            {
                var isMeasureType = IsDimensionType(property.PropertyType);
                if (!isMeasureType) continue;
                result.Add(new QueryProperty()
                {
                    Key = property.Name
                });
            }
            return result;
        }


        internal static List<QueryProperty> ListTimeDimensionProperties<T>()
        {
            return ListTimeDimensionProperties(typeof(T));
        }
        internal static List<QueryProperty> ListTimeDimensionProperties(string queryKey)
        {
            var type = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(e => e.GetTypes())
                .Where(e => e.IsDefined(typeof(QueryAttribute), true))
                .FirstOrDefault(e => e.GetCustomAttribute<QueryAttribute>()?.Key == queryKey);

            return ListTimeDimensionProperties(type);
        }
        internal static List<QueryProperty> ListTimeDimensionProperties(Type type)
        {
            var flags = BindingFlags.Public | BindingFlags.Instance;

            var annotatedProperties = type.GetProperties(flags)
                .Where(e => e.IsDefined(typeof(QueryTimeDimensionAttribute), true))
                .Select(e => new QueryProperty()
                {
                    Key = e.GetCustomAttribute<QueryTimeDimensionAttribute>()?.Key ?? e.Name,
                    DisplayName = e.GetCustomAttribute<QueryTimeDimensionAttribute>()?.DisplayName
                }).ToList();
            if (annotatedProperties.Count != 0)
            {
                return annotatedProperties;
            }

            var result = new List<QueryProperty>();
            var properties = type.GetProperties(flags);
            foreach (var property in properties)
            {
                var isMeasureType = IsTimeDimensionType(property.PropertyType);
                if (!isMeasureType) continue;
                result.Add(new QueryProperty()
                {
                    Key = property.Name
                });
            }
            return result;
        }


        public static bool IsMeasureType(Type type)
        {
            return new[]
            {
                typeof(string),
                typeof(char),
                typeof(byte),
                typeof(sbyte),
                typeof(ushort),
                typeof(short),
                typeof(uint),
                typeof(int),
                typeof(ulong),
                typeof(long),
                typeof(float),
                typeof(double),
                typeof(decimal),
                typeof(DateTime)
            }.Contains(type);
        }

        public static bool IsDimensionType(Type type)
        {
            return new[]
            {
                typeof(string),
                typeof(char),
                typeof(byte),
                typeof(sbyte),
                typeof(ushort),
                typeof(short),
                typeof(uint),
                typeof(int),
                typeof(ulong),
                typeof(long),
                typeof(float),
                typeof(double),
                typeof(decimal),
                typeof(DateTime)
            }.Contains(type);
        }

        private static bool IsTimeDimensionType(Type type)
        {
            return new[] { typeof(DateTime) }.Contains(type);
        }

    }
}
