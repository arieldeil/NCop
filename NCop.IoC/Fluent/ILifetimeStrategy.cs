﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCop.IoC.Fluent
{
    internal interface ILifetimeStrategy : IFlentInterface
    {
        void AsSingleton();
    }
}