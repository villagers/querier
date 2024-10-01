using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Options
{
    public class QuerierOption
    {
        public required bool Enabled { get; set; }
        public required string LocalStoragePath { set; get; }
    }
}
