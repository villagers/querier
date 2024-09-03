using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Interfaces
{
    public interface IPropertyMapper : IPropertyMapperBase
    {
        new IPropertyMapper LoadDefaults();
        new IPropertyMapper LoadType<TType>();
        new IPropertyMapper LoadType(Type type);
        new IPropertyMapper LoadAssembly(string assemblyToLoad);
        new IPropertyMapper LoadNamespace(string namespaceToLoad);
    }
}
