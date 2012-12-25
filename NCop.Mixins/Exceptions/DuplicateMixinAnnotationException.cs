// ------------------------------------------------------------------------------
//  <auto-generated>
//    Generated by NCop Copyright � 2012
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
	public class DuplicateMixinAnnotationException : ArgumentException
	{
		private static string _message = "Duplicate mixin annotation has been found";
		
		public DuplicateMixinAnnotationException(string parameterName) : base(_message, parameterName) { }

		public DuplicateMixinAnnotationException(Exception innerException) : this(_message, innerException) { }

		public DuplicateMixinAnnotationException(string parameterName, string message) : base(message, parameterName) { }

		public DuplicateMixinAnnotationException(string message, Exception innerException) : base(message, innerException) { }

		protected DuplicateMixinAnnotationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}
	