﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCop.Aspects.Weaving
{
    internal interface IAspectArgumentReflector
    {
        Type ArgumentType { get; }
    }
}
