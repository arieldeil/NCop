﻿using NCop.Aspects.Engine;
using NCop.Core.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCop.Aspects.Runtime
{
    public interface IAspectBuilderRegistry
    {
        void RegisterBuilder(Type type, IAspectBuilder builder);
    }
}