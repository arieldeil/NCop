﻿using NCop.Aspects.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCop.Aspects.Framework
{
    public class TransientAspectAttribute : AspectLifetimeStrategyAttribute
    {
        public override LifetimeStrategy LifetimeStrategy {
            get {
                return LifetimeStrategy.Transient;
            }
        }
    }
}
