﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;

namespace NCop.Core.Extensions
{
    public static class ReflectionExtensions
    {
        internal static readonly string NCopToken = "5f8f9ac08842d356";
        private static readonly Regex publicKeyTokenValue = new Regex(@"PublicKeyToken=(?<PublicKeyTokenValue>[A-Fa-f0-9]{16})");

#if !NET_4_5

        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>(this ICustomAttributeProvider type, bool inherit = true) where TAttribute : Attribute {
            return type.GetCustomAttributes(typeof(TAttribute), inherit)
                       .Cast<TAttribute>();
        }

        public static TAttribute GetCustomAttribute<TAttribute>(this ICustomAttributeProvider type, bool inherit = true) where TAttribute : Attribute {
            return type.GetCustomAttributes<TAttribute>(inherit).FirstOrDefault();
        }

#endif

        public static TAttribute[] GetCustomAttributesArray<TAttribute>(this Type type, bool inherit = true) where TAttribute : Attribute {
            return type.GetCustomAttributes<TAttribute>(inherit).ToArray();
        }

        public static Attribute GetCustomAttribute(this ICustomAttributeProvider type, ISet<Type> attributesToMatch, bool inherit = true) {
            return type.GetCustomAttributes(inherit)
                       .FirstOrDefault(attr => attributesToMatch.Contains(attr)) as Attribute;
        }

        public static string GetAssemblyPublicKeyToken(this Type type) {
            return type.Assembly.GetPublicKeyToken();
        }

        public static string GetPublicKeyToken(this Assembly assembly) {
            return publicKeyTokenValue.Match(assembly.FullName).Groups["PublicKeyTokenValue"].Value;
        }

        public static bool HasSamePublicKeyToken(this Assembly assembly, string token) {
            return assembly.GetPublicKeyToken().Equals(token);
        }

        public static bool HasSamePublicKeyToken(this Assembly assembly, params string[] tokens) {
            return tokens.Any(token => assembly.HasSamePublicKeyToken(token));
        }

        public static bool IsNCopAssembly(this Assembly assembly) {
            return assembly.GetPublicKeyToken().Equals(NCopToken);
        }

        public static bool InNCopAssembly(this Type type) {
            return type.Assembly.IsNCopAssembly();
        }

        public static bool IsDefined<TAttribute>(this Type type, bool inherit = true) where TAttribute : Attribute {
            return type.IsDefined(typeof(TAttribute), inherit);
        }

        public static bool IsDefined<TAttribute>(this ConstructorInfo ctor, bool inherit = true) where TAttribute : Attribute {
            return ctor.IsDefined(typeof(TAttribute), inherit);
        }

        public static bool IsDefined<TAttribute>(this ParameterInfo parameter, bool inherit = true) where TAttribute : Attribute {
            return parameter.IsDefined(typeof(TAttribute), inherit);
        }

        public static bool IsDefined<TAttribute>(this MemberInfo memberInfo, bool inherit = true) where TAttribute : Attribute {
            return memberInfo.IsDefined(typeof(TAttribute), inherit);
        }

        public static bool IsNCopDefined<TAttribute>(this Type type, bool inherit = true) where TAttribute : Attribute {
            return type.IsDefined(typeof(TAttribute), inherit) &&
                   type.GetCustomAttribute<TAttribute>(true)
                       .GetType()
                       .InNCopAssembly();
        }

        public static IEnumerable<Type> GetImmediateInterfaces(this Type type) {
            var interfaces = type.GetInterfaces();
            var nonInheritedInterfaces = new HashSet<Type>(interfaces);

            foreach (var @interface in interfaces) {
                @interface.RemoveInheritedInterfaces(nonInheritedInterfaces);
            }

            return nonInheritedInterfaces;
        }

        public static bool HasCovariantType(this ISet<Type> set, Type inspected) {
            return set.Any(item => {
                return inspected.IsCovariantTo(item);
            });
        }

        private static void RemoveInheritedInterfaces(this Type type, HashSet<Type> interfaces) {
            foreach (var @interface in type.GetInterfaces()) {
                interfaces.Remove(@interface);
                RemoveInheritedInterfaces(@interface, interfaces);
            }
        }

        public static MethodInfo[] GetOverridenMethods(this Type type) {
            return type.GetMethods()
                       .Where(method => method.IsOverride(type))
                       .ToArray();
        }

        public static bool IsOverride(this MethodInfo methodInfo, Type declaringType) {
            return methodInfo.DeclaringType == declaringType;
        }

        public static TypeBuilder SetCustomAttribute<TAttribute>(this TypeBuilder builder) where TAttribute : Attribute {
            return builder.SetCustomAttribute<TypeBuilder, TAttribute>(builder.SetCustomAttribute);
        }

        public static ParameterBuilder SetCustomAttribute<TAttribute>(this ParameterBuilder builder) where TAttribute : Attribute {
            return builder.SetCustomAttribute<ParameterBuilder, TAttribute>(builder.SetCustomAttribute);
        }

        private static TBuilder SetCustomAttribute<TBuilder, TAttribute>(this TBuilder builder, Action<CustomAttributeBuilder> customBuilder) where TAttribute : Attribute {
            var ctor = typeof(TAttribute).GetConstructor(Type.EmptyTypes);

            customBuilder(new CustomAttributeBuilder(ctor, Type.EmptyTypes));

            return builder;
        }

        public static bool IsNull(this object value) {
            return object.ReferenceEquals(value, null);
        }

        public static bool IsNotNull(this object value) {
            return !value.IsNull();
        }

        public static PropertyInfo[] GetPublicProperties(this Type type) {
            if (type.IsInterface) {
                var propertyInfos = new List<PropertyInfo>();
                var considered = new List<Type>();
                var queue = new Queue<Type>();

                considered.Add(type);
                queue.Enqueue(type);

                while (queue.Count > 0) {
                    var subType = queue.Dequeue();

                    foreach (var subInterface in subType.GetInterfaces()) {
                        if (considered.Contains(subInterface)) {
                            continue;
                        }

                        considered.Add(subInterface);
                        queue.Enqueue(subInterface);
                    }

                    var typeProperties = subType.GetTypePublicProperties();
                    var newPropertyInfos = typeProperties.Where(x => !propertyInfos.Contains(x));

                    propertyInfos.InsertRange(0, newPropertyInfos);
                }

                return propertyInfos.ToArray();
            }

            return type.GetTypePublicProperties()
                       .ToArray(t => t.GetIndexParameters().Length == 0);
        }

        public static PropertyInfo GetPublicProperty(this Type type, string name) {
            return type.GetPublicProperties().FirstOrDefault(prop => prop.Name.Equals(name));
        }

        internal static PropertyInfo[] GetTypePublicProperties(this Type subType) {
            return subType.GetProperties(BindingFlags.FlattenHierarchy |
                                         BindingFlags.Public |
                                         BindingFlags.Instance);
        }

        public static bool IsCovariantTo(this Type type, Type inspected) {
            return TypeComparer.Compare(type, inspected);
        }

        public static bool HasReturnType(this MethodInfo methodInfo) {
            return !methodInfo.ReturnType.Equals(typeof(void));
        }

        public static bool Is<TCompareTo>(this object @object) {
            return typeof(TCompareTo).IsAssignableFrom(@object.GetType());
        }
        
        public static Type GetDelegateType(this Type[] parameters, bool isFunction) {
            var delegateFactory = isFunction ? Expression.GetFuncType : (Func<Type[], Type>)Expression.GetActionType;

            return delegateFactory(parameters);
        }

        public static bool IsFunction(this MethodInfo methodInfo) {
            return !methodInfo.ReturnType.Equals(typeof(void));
        }
    }
}