//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.Prism.TestSupport
{
	public class MockUnityContainer : IUnityContainer
	{
		public Dictionary<Type, Type> Types = new Dictionary<Type, Type>();
		public Dictionary<Type, object> Instances = new Dictionary<Type, object>();
		public readonly Dictionary<Type, object> ResolveBag = new Dictionary<Type, object>();

		#region IUnityContainer Members

		public IUnityContainer AddExtension(UnityContainerExtension extension)
		{
			return this;
		}

		public object Configure(Type configurationInterface)
		{
			return null;
		}

		public TConfigurator Configure<TConfigurator>() where TConfigurator : IUnityContainerExtensionConfigurator
		{
			throw new NotImplementedException();
		}

		public IUnityContainer CreateChildContainer()
		{
			throw new NotImplementedException();
		}

		public IUnityContainer Parent
		{
			get { throw new NotImplementedException(); }
		}

		public IEnumerable<ContainerRegistration> Registrations
		{
			get { throw new NotImplementedException(); }
		}

		public IUnityContainer RegisterInstance(Type t, string name, object instance, LifetimeManager lifetime)
		{
			if (!Instances.ContainsKey(t))
				Instances.Add(t, instance);
			else
				Instances[t] = instance;
			return this;
		}

		public object Resolve(Type t, string name, params ResolverOverride[] resolverOverrides)
		{
			if (ResolveBag.ContainsKey(t))
				return ResolveBag[t];

			throw new Exception();
		}

		public IEnumerable<object> ResolveAll(Type t, params ResolverOverride[] resolverOverrides)
		{
			throw new NotImplementedException();
		}

		public object BuildUp(Type t, object existing, string name, params ResolverOverride[] resolverOverrides)
		{
			throw new NotImplementedException();
		}

		public IUnityContainer RegisterType(Type from, Type to, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
		{
			Types.Add(from, to);
			return this;
		}

		public IUnityContainer RemoveAllExtensions()
		{
			throw new NotImplementedException();
		}

		public void Teardown(object o)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}