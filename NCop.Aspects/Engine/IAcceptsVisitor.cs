﻿using NCop.Aspects.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCop.Aspects.Engine
{
    public interface IAcceptsVisitor<TVisitor, out TResult>
    {
        TResult Accept(TVisitor visitor);
    }
}
