﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//    Generated by NCop Copyright © 2013
//    Changes to this file may cause incorrect behavior and will be lost if
//    the code is regenerated.
// </auto-generated
// ------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NCop.Mixins.Exceptions
{
	[Serializable]
	public class MissingMixinException : ArgumentException
	{
		private static string message = "Missing mixin annotation";
		
		public MissingMixinException(string message = null) : base(message ?? MissingMixinException.message) { }

		public MissingMixinException(Exception innerException) : this(message, innerException) { }

		public MissingMixinException(string parameterName, string message = null) : base(message ?? MissingMixinException.message, parameterName) { }

		public MissingMixinException(string message, Exception innerException) : base(message, innerException) { }

		protected MissingMixinException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}
	