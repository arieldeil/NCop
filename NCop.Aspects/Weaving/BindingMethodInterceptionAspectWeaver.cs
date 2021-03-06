﻿using NCop.Aspects.Advices;
using NCop.Aspects.Aspects;
using NCop.Aspects.Extensions;
using NCop.Aspects.Weaving.Expressions;
using NCop.Composite.Weaving;
using NCop.Core.Extensions;
using NCop.Weaving;
using NCop.Weaving.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace NCop.Aspects.Weaving
{
    internal class BindingMethodInterceptionAspectWeaver : AbstractMethodInterceptionAspectWeaver
    {
        protected readonly IArgumentsWeaver argumentsWeaver = null;

        internal BindingMethodInterceptionAspectWeaver(IAspectDefinition aspectDefinition, IAspectWeavingSettings aspectWeavingSettings, FieldInfo weavedType)
            : base(aspectDefinition, aspectWeavingSettings, weavedType) {
            argumentsWeavingSetings.BindingsDependency = weavedType;
            argumentsWeaver = new BindingMethodInterceptionArgumentsWeaver(argumentsWeavingSetings, aspectWeavingSettings);

            if (argumentsWeavingSetings.IsFunction) {
                methodScopeWeavers.Add(new FunctionAspectArgsMappingWeaver(aspectWeavingSettings, argumentsWeavingSetings));
            }
            else {
                methodScopeWeavers.Add(new ActionAspectArgsMappingWeaver(aspectWeavingSettings, argumentsWeavingSetings));
            }

            ArgumentType = argumentsWeavingSetings.ArgumentType;
            weaver = new MethodScopeWeaversQueue(methodScopeWeavers);
        }

        public override ILGenerator Weave(ILGenerator ilGenerator) {
            var isFunction = argumentsWeavingSetings.IsFunction;
            var aspectArgsType = weavingSettings.MethodInfoImpl.ToAspectArgumentContract();

            argumentsWeaver.Weave(ilGenerator);
            weaver.Weave(ilGenerator);

            if (argumentsWeavingSetings.IsFunction) {
                var returnValueProperty = aspectArgsType.GetProperty("ReturnValue");

                ilGenerator.EmitLoadArg(2);
                ilGenerator.Emit(OpCodes.Callvirt, returnValueProperty.GetGetMethod());
            }

            return ilGenerator;
        }
    }
}
