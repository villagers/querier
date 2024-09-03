using Microsoft.Extensions.DependencyInjection;
using Querier.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Options
{
    public class TypeOption
    {
        private readonly IPropertyMapper _propertyMapper;

        public TypeOption(IPropertyMapper propertyMapper)
        {
            _propertyMapper = propertyMapper;
        }

        public IPropertyMapper LoadDefaults() => _propertyMapper.LoadDefaults();
        public IPropertyMapper LoadType(Type type) => _propertyMapper.LoadType(type);
        public IPropertyMapper LoadType<TType>() => _propertyMapper.LoadType<TType>();
        public IPropertyMapper LoadAssembly(string @assembly) => _propertyMapper.LoadAssembly(@assembly);
        public IPropertyMapper LoadNamespace(string @namespace) => _propertyMapper.LoadNamespace(@namespace);
    }
}
