using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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