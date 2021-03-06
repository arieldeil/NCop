﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCop.IoC
{
    internal class ResolveContext<TService>
    {
        internal ServiceKey Key { get; set; }
        internal ServiceEntry Entry { get; set; }
        internal Func<TService> Factory { get; set; }
        internal INCopDependencyResolver Container { get; set; }
        internal Action<ServiceKey, ServiceEntry> Registry { get; set; }
    }
}
