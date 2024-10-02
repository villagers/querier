using System.Reflection;

namespace Querier.Interfaces
{
    public interface IPropertyValidator
    {
        public bool Validate(PropertyInfo propertyInfo);
    }

    public interface IMeasurePropertyValidator : IPropertyValidator { }
    public interface IDimensionPropertyValidator : IPropertyValidator { }
    public interface ITimeDimensionPropertyValidator : IPropertyValidator { }
}