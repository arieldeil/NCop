﻿using NCop.Aspects.Advices;
using NCop.Aspects.Engine;
using NCop.Aspects.Weaving.Expressions;
using System;
using System.Reflection;

namespace NCop.Aspects.Aspects
{
    public interface IAspectDefinition : IAcceptsVisitor<IAspectDefinitionVisitor, IAspectExpressionBuilder>
    {
        IAspect Aspect { get; }
        MemberInfo Member { get; }
        AspectType AspectType { get; }
        Type AspectDeclaringType { get; }
        IAdviceDefinitionCollection Advices { get; }
    }
}
