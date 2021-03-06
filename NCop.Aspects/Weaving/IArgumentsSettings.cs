﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCop.Aspects.Weaving
{
    public interface IArgumentsSettings
    {
        Type ReturnType { get; }
        bool IsFunction { get; }
        Type ArgumentType { get; }
        Type[] Parameters { get; }
    }
}
