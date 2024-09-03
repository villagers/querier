using Querier.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Validators
{
    public class TimeDimensionPropertyValidator : ITimeDimensionPropertyValidator
    {
        public bool Validate(PropertyInfo propertyInfo)
        {
            return new[] { typeof(DateTime) }.Contains(propertyInfo.PropertyType);
        }
    }
}
