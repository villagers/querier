using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Models
{
    public class TypeBase
    {
        public required Type Type { get; set; }
        public required string Key { get; set; }
        public required string Name { get; set; }
        public List<TypePropertyBase> Properties = new List<TypePropertyBase>();
    }
}
