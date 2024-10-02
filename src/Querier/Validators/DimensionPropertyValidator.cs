using Querier.Interfaces;
using System.Reflection;

namespace Querier.Validators
{
    public class DimensionPropertyValidator : IDimensionPropertyValidator
    {
        public bool Validate(PropertyInfo propertyInfo)
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
            }.Contains(propertyInfo.PropertyType);
        }
    }
}
