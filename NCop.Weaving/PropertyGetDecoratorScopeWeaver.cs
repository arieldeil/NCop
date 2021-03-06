﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using NCop.Weaving.Extensions;
using NCop.Core.Extensions;

namespace NCop.Weaving
{
	public class PropertyGetDecoratorScopeWeaver : AbstractMethodScopeWeaver
	{
		public PropertyGetDecoratorScopeWeaver(IWeavingSettings weavingSettings)
			: base(weavingSettings) {
		}

		public override ILGenerator Weave(ILGenerator ilGenerator) {
			FieldBuilder fieldBuilder = TypeDefinition.GetFieldBuilder(ContractType);

			ilGenerator.EmitLoadArg(0);
			ilGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
			ilGenerator.Emit(OpCodes.Callvirt, MethodInfoImpl);

			return ilGenerator;
		}
	}
}
