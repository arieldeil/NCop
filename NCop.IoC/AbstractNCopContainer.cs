﻿using NCop.IoC.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NCop.Core.Extensions;
using System.Threading;
using NCop.IoC.Fluent;
using NCop.Core;

namespace NCop.IoC
{
	public abstract class AbstractNCopContainer : INCopDependencyResolver
	{
		private readonly object syncLock = new object();
		protected readonly IContainerRegistry registry = null;
		protected IDictionary<ServiceKey, ServiceEntry> services = null;
		protected readonly Stack<WeakReference> disposables = new Stack<WeakReference>();

		public AbstractNCopContainer() {
			registry = CreateRegistry();
		}

		protected IDictionary<ServiceKey, ServiceEntry> ResolveServices(IEnumerable<IRegistration> registrations) {
			var dictionary = new Dictionary<ServiceKey, ServiceEntry>();

			registrations.ForEach(registration => {
				var serviceKey = CreateServiceKey(registration);

				if (dictionary.ContainsKey(serviceKey)) {
					throw new RegistrationException(Resources.DuplicateRegistrationFound.Fmt(serviceKey.ServiceType));
				}

				dictionary.Add(serviceKey, CreateServiceEntry(registration));
			});

			return dictionary;
		}

		protected virtual IContainerRegistry CreateRegistry() {
			return new ContainerRegistry();
		}

		public void Configure() {
			ConfigureInternal();
		}

		protected internal void ConfigureInternal() {
			lock (syncLock) {
				services = ResolveServices(registry);
			}
		}

		protected ServiceKey CreateServiceKey(IRegistration registration) {
			return new ServiceKey(registration.ServiceType, registration.FactoryType, registration.Name);
		}

		protected ServiceEntry CreateServiceEntry(IRegistration registration) {
			return new ServiceEntry {
				Owner = registration.Owner,
				Container = this,
				Scope = registration.Scope,
				Factory = registration.Func,
				LifetimeStrategy = registration.Scope.ToStrategy(this)
			};
		}

		private void LateRegister(ServiceKey key, ServiceEntry entry) {
			lock (services) {
				services[key] = entry;
			}
		}

		public TService Resolve<TService>() {
			return ResolveInternal<TService>();
		}

		public TService TryResolve<TService>() {
			return ResolveInternal<TService>(throwIfMissing: false);
		}

		public TService ResolveNamed<TService>(string name) {
			return ResolveInternal<TService>(name: name);
		}

		public TService TryResolveNamed<TService>(string name) {
			return ResolveInternal<TService>(name, false);
		}

		protected TService ResolveInternal<TService>(string name = null, bool throwIfMissing = true) {
			return ResolveImpl<TService, Func<INCopDependencyResolver, TService>>(factory => {
				return factory(this);
			}, name, throwIfMissing);
		}

		protected TService ResolveImpl<TService, TFunc>(Func<TFunc, TService> factoryInvoker, string name = null, bool throwIfMissing = true) {
			ResolveContext<TService> context = null;
			var identifier = new ServiceKey(typeof(TService), typeof(TFunc), name);
			ServiceEntry entry = GetEntry(identifier);

			if (entry.IsNull()) {
				if (throwIfMissing) {
					throw new ResolutionException(Resources.CouldNotResolveType.Fmt(identifier.ServiceType));
				}

				return default(TService);
			}

			context = new ResolveContext<TService> {
				Entry = entry,
				Key = identifier,
				Container = this,
				Registry = LateRegister,
				Factory = Functional.Curry(CreateInstance, entry, (TFunc)entry.Factory, factoryInvoker)
			};

			return entry.LifetimeStrategy.Resolve(context);
		}

		protected TService CreateInstance<TService, TFunc>(ServiceEntry entry, TFunc factory, Func<TFunc, TService> factoryInvoker) {
			var instance = factoryInvoker(factory);

			if (entry.Owner == Owner.Container && instance is IDisposable) {
				lock (disposables) {
					disposables.Push(new WeakReference(instance));
				}
			}

			return instance;
		}

		protected internal bool TryGetEntry(ServiceKey key, out ServiceEntry entry) {
			return (entry = GetEntry(key)) != null;
		}

		protected virtual ServiceEntry GetEntry(ServiceKey key) {
			ServiceEntry entry;

			try {
				if (services.TryGetValue(key, out entry)) {
					return entry;
				}
			}
			catch (NullReferenceException ex) {
				throw new ResolutionException(Resources.ContainerNotConfigured, ex);
			}

			return entry;
		}

		public virtual void Dispose() {
			lock (disposables) {
				while (disposables.Count > 0) {
					var disposable = disposables.Pop();

					if (disposable.IsAlive) {
						((IDisposable)disposable.Target).Dispose();
					}
				}
			}
		}
	}
}
