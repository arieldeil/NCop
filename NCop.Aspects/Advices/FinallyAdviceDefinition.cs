﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NCop.Aspects.Weaving.Expressions;

namespace NCop.Aspects.Advices
{
	internal class FinallyAdviceDefinition : AbstractAdviceDefinition
	{
		private readonly FinallyAdviceAttribute advice = null;

		public FinallyAdviceDefinition(FinallyAdviceAttribute advice, MethodInfo adviceMethod)
			: base(advice, adviceMethod) {
			this.advice = advice;
		}

        public override IAspectExpression Accept(AdviceVisitor visitor) {
			return visitor.Visit(advice).Invoke(this);
		}
	}
}
