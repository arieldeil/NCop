﻿using NCop.Aspects.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NCop.Aspects.Engine
{
    public class FunctionExecutionArgsImpl<TInstance, TArg1, TResult> : FunctionExecutionArgs<TArg1, TResult>, IFunctionArgs<TArg1, TResult>
    {
        public FunctionExecutionArgsImpl(TInstance instance, MethodInfo method, TArg1 arg1) {
            Arg1 = arg1;
            Method = method;
            Instance = instance;
        }
    }
}

