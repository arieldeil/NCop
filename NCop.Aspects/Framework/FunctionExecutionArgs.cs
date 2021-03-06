﻿using NCop.Aspects.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NCop.Aspects.Framework
{
    public abstract class FunctionExecutionArgs<TResult> : AbstractExecutionArgs, IFunctionExecutionArgs
	{
		public TResult ReturnValue { get; set; }
    }
}
