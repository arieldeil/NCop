﻿using NCop.IoC.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCop.IoC
{
    internal static class LifetimeStrategyFactory
    {
        private static IdentityLifetimeStrategy defaultLifetimeStrategy = new IdentityLifetimeStrategy();

        public static ILifetimeStrategy Get(ReuseScope scope) {
            switch (scope) {
                case ReuseScope.None:
                    return defaultLifetimeStrategy;
                
                case ReuseScope.Container:
                    return new SingletonLifetimeSrategy();
                
                default:
                    throw new ResolutionException(Resources.UnknownReuseScope);
            }
        }
    }
}
