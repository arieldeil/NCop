﻿using NCop.Weaving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NCop.Aspects.Advices;

namespace NCop.Aspects.Weaving.Expressions
{
    internal class OnMethodSuccessAdviceExpression : AbstractAdviceExpression
    {
        internal OnMethodSuccessAdviceExpression(IAdviceDefinition adviceDefinition)
            : base(adviceDefinition) {
        }

        protected override AdviceType AdviceType {
            get {
                return AdviceType.OnMethodSuccessAdvice;
            }
        }

        public override IMethodScopeWeaver Reduce(IAdviceWeavingSettings adviceWeavingSettings) {
            return new OnMethodSuccessAdviceWeaver(adviceWeavingSettings);
        }
    }
}