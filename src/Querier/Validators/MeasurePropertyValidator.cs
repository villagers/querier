using Querier.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Validators
{
    public class MeasurePropertyValidator : IMeasurePropertyValidator
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
