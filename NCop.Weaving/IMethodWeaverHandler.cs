﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NCop.Weaving
{
    public interface IMethodWeaverHandler
    {
        IMethodWeaver Handle(MethodInfo methodInfo, ITypeDefinition typeDefinition);
    }
}
