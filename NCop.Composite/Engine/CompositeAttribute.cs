﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCop.Composite.Engine
{
    [AttributeUsage(AttributeTargets.Interface)]
    public abstract class CompositeAttribute : Attribute
    {
        public Type As { get; set; }
    }
}
