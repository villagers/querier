using Querier.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Options
{
    public class QuerierUseOption
    {
        public IPropertyMapper Types { private set; get; }
        public QuerierUseOption(IPropertyMapper mapper)
        {
            Types = mapper;
        }
    }
}
