using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Models
{
    public class TypePropertyBase
    {
        public required string Key { get; set; }
        public required string Name { get; set; }
        public List<Type> AttributeTypes = new List<Type>();
        public Dictionary<string, string> Attributes = new Dictionary<string, string>();
    }
}
