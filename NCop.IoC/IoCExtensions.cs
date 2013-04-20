﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCop.IoC
{
    public static class IoCExtensions
    {
        internal static ILifetimeStrategy ToStrategy(this ReuseScope scope) {
            return LifetimeStrategyFactory.Get(scope);
        }
    }
}
