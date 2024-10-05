using Querier.Interfaces;
using System.Reflection;

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
