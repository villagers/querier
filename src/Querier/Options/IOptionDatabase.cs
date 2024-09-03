using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Options
{
    public interface IOptionDatabase
    {
        void UseMySql(string connectionString);
    }
}
