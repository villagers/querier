using MySqlX.XDevAPI.Common;
using Querier.Attributes;
using Querier.Helpers;
using Querier.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Querier
{
    public class PropertyMapper : PropertyMapperBase, IPropertyMapper
    {
        public PropertyMapper(IMeasurePropertyValidator measurePropertyValidator, IDimensionPropertyValidator dimensionPropertyValidator, ITimeDimensionPropertyValidator timeDimensionPropertyValidator)
            : base(measurePropertyValidator, dimensionPropertyValidator, timeDimensionPropertyValidator) { }

        public new IPropertyMapper LoadType(Type type)
        {
            base.LoadType(type);
            return this;
        }

        public new IPropertyMapper LoadDefaults()
        {
            base.LoadDefaults();
            return this;
        }

        public new IPropertyMapper LoadType<TType>()
        {
            base.LoadType<TType>();
            return this;
        }

        public new IPropertyMapper LoadAssembly(string assemblyToLoad)
        {
            base.LoadAssembly(assemblyToLoad);
            return this;
        }

        public new IPropertyMapper LoadNamespace(string namespaceToLoad)
        {
            base.LoadNamespace(namespaceToLoad);
            return this;
        }
    }
}
