﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Attributes
{
    public interface IDisplayAttribute
    {
        string DisplayName { get; set; }
    }
}