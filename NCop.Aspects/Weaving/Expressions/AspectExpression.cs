﻿using NCop.Aspects.Aspects;
using NCop.Aspects.Extensions;
using NCop.Weaving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NCop.Aspects.Weaving.Expressions
{
    internal class AspectExpression : IAspectExpression
    {
        private readonly IWeavingSettings weavingSettings = null;
        private readonly IAspectExpression aspectExpression = null;
        private readonly IAspectDefinitionCollection aspectsDefinitions = null;

        internal AspectExpression(IAspectExpression aspectExpression, IAspectDefinitionCollection aspectsDefinitions, IWeavingSettings weavingSettings) {
			this.weavingSettings = weavingSettings;
            this.aspectExpression = aspectExpression;
            this.aspectsDefinitions = aspectsDefinitions;
        }

        public IAspectWeaver Reduce(IAspectWeavingSettings aspectWeavingSettings ) {
            return new AspectsWeaver(aspectExpression, aspectsDefinitions, weavingSettings);
        }
    }
}
