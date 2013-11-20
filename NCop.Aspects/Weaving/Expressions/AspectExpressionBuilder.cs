﻿using NCop.Aspects.Aspects;
using NCop.Aspects.Framework;
using NCop.Weaving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NCop.Aspects.Weaving.Expressions
{
	internal class AspectExpression : AbstractAspectExpression
	{
		internal AspectExpression(IEnumerable<IAspectDefinition> aspectDefinitions) {
			var aspectsByPriority = aspectDefinitions.OrderBy(aspect => aspect.Aspect.AspectPriority)
													 .ThenBy(aspect => {
														 var value = aspect.Aspect is OnMethodBoundaryAspectAttribute;
														 return Convert.ToInt32(!value);
													 });

			var aspectsQueue = new Queue<IAspectDefinition>(aspectsByPriority);

			BuildExpressionTree(aspectsQueue);
		}

		private void BuildExpressionTree(Queue<IAspectDefinition> aspectsQueue) {
			IAspectExpression current = null;
			var visitor = new AspectAttributeVisitor();
			var aspectDefinition = aspectsQueue.Dequeue();

			Expression = current = visitor.Visit(aspectDefinition);

			while (aspectsQueue.Count > 0) {
				aspectDefinition = aspectsQueue.Dequeue();	
				current.Expression = visitor.Visit(aspectDefinition);
				current = current.Expression;
			}
		}

		public override IMethodScopeWeaver Reduce() {
			throw new NotImplementedException();
		}
	}
}