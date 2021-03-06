﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NCop.Core.Extensions;
using System.Reflection;
using NCop.IoC.Framework;
using NCop.IoC.Properties;

namespace NCop.IoC.Fluent
{
	public class AutoRegistration<TService> : CastableRegistration<TService>
	{
		public AutoRegistration(Type serviceType, Type factoryType)
			: base(serviceType, factoryType) {
		}

		protected override ICasted As(Type castTo) {
			var typeofService = typeof(TService);
			var containerParamater = Expression.Parameter(typeof(INCopDependencyResolver), "container");
			var tryResolveMethodInfo = typeof(INCopDependencyResolver).GetMethod("TryResolve", Type.EmptyTypes);
			var dependencyAware = typeofService.IsDefined<DependencyAware>();
			var properties = castTo.GetPublicProperties()
								   .Where(prop => prop.CanWrite)
								   .Where(prop => !prop.PropertyType.IsValueType)
								   .Where(prop => !prop.PropertyType.Equals(typeof(string)))
								   .Where(prop => !dependencyAware && !prop.IsDefined<IgnoreDependency>() ||
												  dependencyAware && prop.IsDefined<DependencyAttribute>());

			var assignments = properties.Select(prop => {
				var type = prop.PropertyType;
				var methodCallExpression = MethodCallExpression(tryResolveMethodInfo, type, containerParamater);

				return Expression.Bind(prop, methodCallExpression);
			});

			var ctorResolveMethodInfo = typeof(INCopDependencyResolver).GetMethod("Resolve", Type.EmptyTypes);
			var newExpression = NewExpression(ctorResolveMethodInfo, castTo, containerParamater);
			var lambda = Expression.Lambda(
							Expression.MemberInit(newExpression, assignments.ToArray()),
								  containerParamater);

			Registration.Func = lambda.Compile();

			return this;
		}

		private NewExpression NewExpression(MethodInfo methodInfo, Type type, ParameterExpression instance) {
			var ctor = GetSingleConstructorOrAnnotated(type);
			var @params = ctor.GetParameters()
							  .Select(pi => {
								  return MethodCallExpression(methodInfo, pi.ParameterType, instance);
							  });

			return Expression.New(ctor, @params.ToArray());
		}

		private MethodCallExpression MethodCallExpression(MethodInfo methodInfo, Type parameterType, ParameterExpression instance) {
			var method = methodInfo.MakeGenericMethod(parameterType);

			return Expression.Call(instance, method);
		}

		private ConstructorInfo GetSingleConstructorOrAnnotated(Type type) {
			var ctors = type.GetConstructors();

			if (ctors.Length > 1) {
				var dependentCtors = ctors.ToArray(ctor => ctor.IsDefined<DependencyAttribute>());

				if (dependentCtors.Length != 1) {
					throw new RegistrationException(Resources.AmbigiousConstructorDependency.Fmt(type));
				}

				ctors = dependentCtors;
			}

			return ctors[0];
		}
	}
}
