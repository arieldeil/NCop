﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NCop.Core;
using NCop.Core.Extensions;
using NCop.Aspects.Engine;
using NCop.Aspects.Aspects;

namespace NCop.Composite.Engine
{
    public class CompositeMethodMap : AbstractMemberMap<MethodInfo>, ICompositeMethodMap
    {
        public CompositeMethodMap(Type contractType, Type implementationType, MethodInfo contractMethod, MethodInfo implementationMethod, IAspectDefinitionCollection aspectDefinitions)
            : base(contractType, implementationType, contractMethod, implementationMethod) {
            AspectDefinitions = aspectDefinitions;
            HasAspectDefinitions = aspectDefinitions.IsNotNullOrEmpty();
        }

        public bool HasAspectDefinitions { get; private set; }
        
        public IAspectDefinitionCollection AspectDefinitions { get; private set; }
    }
}
