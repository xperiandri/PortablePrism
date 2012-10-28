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

using Microsoft.Practices.Prism.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.Storage;

namespace Microsoft.Practices.Prism.Modularity
{
    /// <summary>
    /// Handles AppDomain's AssemblyResolve event to be able to load assemblies dynamically in
    /// the LoadFrom context, but be able to reference the type from assemblies loaded in the Load context.
    /// </summary>
    public class AssemblyResolver : IAssemblyResolver
    {
        private readonly List<AssemblyInfo> loadedAssemblies = new List<AssemblyInfo>();

        /// <summary>
        /// Registers the specified assembly and resolves the types in it when the AppDomain requests for it.
        /// </summary>
        /// <param name="assemblyFilePath">The path to the assemly to load in the LoadFrom context.</param>
        /// <remarks>This method does not load the assembly immediately, but lazily until someone requests a <see cref="Type"/>
        /// declared in the assembly.</remarks>
        public async void LoadAssemblyFrom(string assemblyFilePath)
        {
            //if (!this.handlesAssemblyResolve)
            //{
            //    AppDomain.CurrentDomain.AssemblyResolve += this.CurrentDomain_AssemblyResolve;
            //    this.handlesAssemblyResolve = true;
            //}

            Uri assemblyUri = GetFileUri(assemblyFilePath);

            if (assemblyUri == null)
            {
                throw new ArgumentException(ResourceHelper.InvalidArgumentAssemblyUri, "assemblyFilePath");
            }

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(assemblyUri.AbsoluteUri));

            AssemblyName assemblyName = new AssemblyName { Name = file.DisplayName };
            if (this.loadedAssemblies.Any(a => assemblyName == a.AssemblyName))
            {
                return;
            }

            var assembly = Assembly.Load(assemblyName);
            var assemblyInfo = new AssemblyInfo() { AssemblyName = assemblyName, AssemblyUri = assemblyUri, Assembly = assembly };
            this.loadedAssemblies.Add(assemblyInfo);
        }

        private static Uri GetFileUri(string filePath)
        {
            if (String.IsNullOrEmpty(filePath))
            {
                return null;
            }

            Uri uri;
            if (!Uri.TryCreate(filePath, UriKind.Absolute, out uri))
            {
                return null;
            }

            if (!uri.IsFile)
            {
                return null;
            }

            return uri;
        }

        private class AssemblyInfo
        {
            public AssemblyName AssemblyName { get; set; }

            public Uri AssemblyUri { get; set; }

            public Assembly Assembly { get; set; }
        }
       
    }
}