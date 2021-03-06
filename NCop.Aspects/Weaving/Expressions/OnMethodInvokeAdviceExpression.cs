﻿using NCop.Weaving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NCop.Aspects.Advices;

namespace NCop.Aspects.Weaving.Expressions
{
    internal class OnMethodInvokeAdviceExpression : AbstractAdviceExpression
    {
        internal OnMethodInvokeAdviceExpression(IAdviceDefinition adviceDefinition)
            : base(adviceDefinition) {
        }

        protected override AdviceType AdviceType {
            get {
                return AdviceType.OnMethodInvokeAdvice;
            }
        }

        public override IMethodScopeWeaver Reduce(IAdviceWeavingSettings adviceWeavingSettings) {
            return new OnMethodInvokeWeaver(adviceWeavingSettings);
        }
    }
}