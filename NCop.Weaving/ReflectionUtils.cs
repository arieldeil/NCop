﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using NCop.Core.Extensions;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace NCop.Weaving
{
	public static class ReflectionUtils
	{
		public static MethodBuilder DefineMethod(this TypeBuilder typeBuilder, MethodInfo methodInfo, MethodAttributes? attributes = null, ParameterInfo[] parameters = null) {
			Type[] parametersTypes = null;
			MethodBuilder methodBuilder = null;

			parameters = parameters ?? methodInfo.GetParameters();
			parametersTypes = parameters.ToArray(parameter => parameter.ParameterType);
			attributes = attributes ?? methodInfo.Attributes & ~MethodAttributes.Abstract;
			methodBuilder = typeBuilder.DefineMethod(methodInfo.Name, attributes.Value, methodInfo.ReturnType, parametersTypes);

			parameters.ForEach(1, (parameter, i) => {
				var parameterBuilder = methodBuilder.DefineParameter(i, parameter.Attributes, parameter.Name);

				if (parameter.IsDefined<ParamArrayAttribute>()) {
					parameterBuilder.SetCustomAttribute<ParamArrayAttribute>();
				}
				else if (parameter.IsOut) {
					parameterBuilder.SetCustomAttribute<OutAttribute>();
				}
				else if (parameter.IsOptional) {
					parameterBuilder.SetCustomAttribute<OptionalAttribute>()
									.SetConstant(parameter.DefaultValue);
				}
			});

			return methodBuilder;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static MethodBuilder DefineParameterlessMethod(this TypeBuilder typeBuilder, MethodInfo methodInfo, MethodAttributes? attributes = null) {
			attributes = attributes ?? methodInfo.Attributes & ~MethodAttributes.Abstract;

			return typeBuilder.DefineMethod(methodInfo.Name, attributes.Value, methodInfo.ReturnType, Type.EmptyTypes);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public static PropertyBuilder DefineProperty(this TypeBuilder typeBuilder, PropertyInfo propertyInfo, string name = null, PropertyAttributes? attributes = null, Type propertyType = null) {
			name = name ?? propertyInfo.Name;
			attributes = attributes ?? propertyInfo.Attributes;
			propertyType = propertyType ?? propertyInfo.PropertyType;

			return typeBuilder.DefineProperty(name, attributes.Value, propertyType, Type.EmptyTypes);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public static TypeBuilder DefineType(this Type parentType, string name = null, IEnumerable<Type> interfaces = null, TypeAttributes? attributes = null) {
			var moudleBuilder = NCopModuleBuilder.Instance;
			name = name ?? parentType.ToUniqueName();
			attributes = attributes ?? parentType.Attributes;

			if (interfaces.IsNotNullOrEmpty()) {
				return moudleBuilder.DefineType(name, attributes.Value, parentType, interfaces.ToArray());
			}

			return moudleBuilder.DefineType(name, attributes.Value, parentType);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public static string ToUniqueName(this string name) {
			return string.Format("<NCop>.{0}", name);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public static string ToUniqueName(this Type type) {
			return type.FullName.ToUniqueName();
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public static string ToFieldName(this Type type) {
			string name = type.Name;

			if (type.IsInterface && name[0] == 'I' && name.Length > 2) {
				var chars = name.Skip(2);
				var builder = new StringBuilder().Append(char.ToLower(name[1]));

				chars.ForEach(c => builder.Append(c));
				name = builder.ToString();
			}

			return name;
		}

		internal static FieldBuilder DefineField(this TypeBuilder typeBuilder, Type fieldType, string fieldName = null, FieldAttributes? attributes = null) {
			string name = fieldName ?? fieldType.ToFieldName();

			attributes = attributes ?? FieldAttributes.Private;

			return typeBuilder.DefineField(name, fieldType, attributes.Value);
		}

		internal static Type GetNonNullableType(this Type type) {
			if (type.IsNullableType()) {
				return type.GetGenericArguments()[0];
			}

			return type;
		}

		internal static bool IsInt32OrInt64(this Type type) {
			return type == typeof(int) || type == typeof(long);
		}

		internal static bool IsSingleOrDouble(this Type type) {
			return type == typeof(float) || type == typeof(double);
		}

		internal static bool IsFloatingPoint(this Type type) {
			type = type.GetNonNullableType();

			switch (Type.GetTypeCode(type)) {
				case TypeCode.Single:
				case TypeCode.Double:

					return true;

				default:

					return false;
			}
		}

		internal static bool IsUnsigned(this Type type) {
			type = type.GetNonNullableType();

			switch (Type.GetTypeCode(type)) {
				case TypeCode.Char:
				case TypeCode.Byte:
				case TypeCode.UInt16:
				case TypeCode.UInt32:
				case TypeCode.UInt64:
					return true;
			}
			return false;
		}

		internal static bool IsNullableType(this Type type) {
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		internal static bool IsEquivalentTo(this Type type1, Type type2) {
			if (!(type1 == type2)) {
				return type1.IsEquivalentTo(type2);
			}

			return true;
		}

		internal static bool ContainsGenericParameters(this Type type, out Type[] arguments) {
			arguments = Type.EmptyTypes;

			if (type.IsGenericType) {
				if (type.ContainsGenericParameters) {
					arguments = type.GetGenericArguments();
				}

				return true;
			}

			return false;
		}

		internal static bool IsPrimitive(this Type type) {
			return type.IsPrimitive || type.IsValueType || Type.GetTypeCode(type) == TypeCode.Decimal;
		}

		internal static bool IsNumeric(this Type type) {
			type = type.GetNonNullableType();

			if (type.IsEnum) {
				return true;
			}

			switch (Type.GetTypeCode(type)) {
				case TypeCode.Boolean:
				case TypeCode.Char:
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
				case TypeCode.UInt64:
				case TypeCode.Single:
				case TypeCode.Double:
					return true;
			}

			return false;
		}
	}
}
