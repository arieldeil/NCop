﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCop.Aspects.Framework
{
    public interface IMethodExecutionArgs
    {
        FlowBehavior FlowBehavior { get; }
    }
}
