﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCop.Aspects.Framework;
using System.Reflection;

namespace NCop.Aspects.Engine
{
    public class ActionExecutionArgsImpl<TInstance, TArg1, TArg2, TArg3> : ActionExecutionArgs<TArg1, TArg2, TArg3>, IActionArgs<TArg1, TArg2, TArg3>
    {
        public ActionExecutionArgsImpl(TInstance instance, MethodInfo method, TArg1 arg1, TArg2 arg2, TArg3 arg3) {
            Arg1 = arg1;
            Arg2 = arg2;
            Arg3 = arg3;
            Method = method;
            Instance = instance;
        }
    }
}
