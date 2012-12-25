﻿using NCop.Aspects.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCop.Aspects.Aspects.Builders
{
    public interface IAspectBuilder
    {
        IAspectDefinitionCollection Build(Type type);
    }
}