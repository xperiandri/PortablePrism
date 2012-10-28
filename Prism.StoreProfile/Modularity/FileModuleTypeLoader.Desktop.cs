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
using System.IO;
using System.Collections.Generic;
using Windows.Storage;
using System.Diagnostics.Contracts;

namespace Microsoft.Practices.Prism.Modularity
{
    /// <summary>
    /// Loads modules from an arbitrary location on the filesystem. This typeloader is only called if 
    /// <see cref="ModuleInfo"/> classes have a Ref parameter that starts with "file://". 
    /// This class is only used on the Desktop version of the Composite Application Library.
    /// </summary>
    public class FileModuleTypeLoader : IModuleTypeLoader, IDisposable
    {
        private const string RefFilePrefix = "ms-appx:///";

        private readonly IAssemblyResolver assemblyResolver;
        private HashSet<Uri> downloadedUris = new HashSet<Uri>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileModuleTypeLoader"/> class.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "This is disposed of in the Dispose method.")]
        public FileModuleTypeLoader()
            : this(new AssemblyResolver())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileModuleTypeLoader"/> class.
        /// </summary>
        /// <param name="assemblyResolver">The assembly resolver.</param>
        public FileModuleTypeLoader(IAssemblyResolver assemblyResolver)
        {
            this.assemblyResolver = assemblyResolver;
        }

        /// <summary>
        /// Raised repeatedly to provide progress as modules are loaded in the background.
        /// </summary>
        public event EventHandler<ModuleDownloadProgressChangedEventArgs> ModuleDownloadProgressChanged;

        private void RaiseModuleDownloadProgressChanged(ModuleInfo moduleInfo, long bytesReceived, long totalBytesToReceive)
        {
            this.RaiseModuleDownloadProgressChanged(new ModuleDownloadProgressChangedEventArgs(moduleInfo, bytesReceived, totalBytesToReceive));
        }

        private void RaiseModuleDownloadProgressChanged(ModuleDownloadProgressChangedEventArgs e)
        {
            if (this.ModuleDownloadProgressChanged != null)
            {
                this.ModuleDownloadProgressChanged(this, e);
            }
        }

        /// <summary>
        /// Raised when a module is loaded or fails to load.
        /// </summary>
        public event EventHandler<LoadModuleCompletedEventArgs> LoadModuleCompleted;

        private void RaiseLoadModuleCompleted(ModuleInfo moduleInfo, Exception error)
        {
            this.RaiseLoadModuleCompleted(new LoadModuleCompletedEventArgs(moduleInfo, error));
        }

        private void RaiseLoadModuleCompleted(LoadModuleCompletedEventArgs e)
        {
            if (this.LoadModuleCompleted != null)
            {
                this.LoadModuleCompleted(this, e);
            }
        }

        /// <summary>
        /// Evaluates the <see cref="ModuleInfo.Ref"/> property to see if the current typeloader will be able to retrieve the <paramref name="moduleInfo"/>.
        /// Returns true if the <see cref="ModuleInfo.Ref"/> property starts with "file://", because this indicates that the file
        /// is a local file. 
        /// </summary>
        /// <param name="moduleInfo">Module that should have it's type loaded.</param>
        /// <returns>
        /// 	<see langword="true"/> if the current typeloader is able to retrieve the module, otherwise <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">An <see cref="ArgumentNullException"/> is thrown if <paramref name="moduleInfo"/> is null.</exception>
        public bool CanLoadModuleType(ModuleInfo moduleInfo)
        {
            return moduleInfo.Ref != null && moduleInfo.Ref.StartsWith(RefFilePrefix, StringComparison.Ordinal);
        }


        /// <summary>
        /// Retrieves the <paramref name="moduleInfo"/>.
        /// </summary>
        /// <param name="moduleInfo">Module that should have it's type loaded.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Exception is rethrown as part of a completion event")]
        public async void LoadModuleType(ModuleInfo moduleInfo)
        {
            try
            {
                Uri uri = new Uri(moduleInfo.Ref, UriKind.RelativeOrAbsolute);                

                // If this module has already been downloaded, I fire the completed event.
                if (this.IsSuccessfullyDownloaded(uri))
                {
                    this.RaiseLoadModuleCompleted(moduleInfo, null);
                }
                else
                {
                    string path = moduleInfo.Ref.Substring(RefFilePrefix.Length + 1);

                    long fileSize = -1L;
                    try
                    {
                        StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(path));
                        var properties = await file.GetBasicPropertiesAsync();
                        fileSize = Convert.ToInt64(properties.Size);
                    }
                    catch (FileNotFoundException)
                    {
                        
                    }

                    // Although this isn't asynchronous, nor expected to take very long, I raise progress changed for consistency.
                    this.RaiseModuleDownloadProgressChanged(moduleInfo, 0, fileSize);

                    this.assemblyResolver.LoadAssemblyFrom(moduleInfo.Ref);

                    // Although this isn't asynchronous, nor expected to take very long, I raise progress changed for consistency.
                    this.RaiseModuleDownloadProgressChanged(moduleInfo, fileSize, fileSize);

                    // I remember the downloaded URI.
                    this.RecordDownloadSuccess(uri);

                    this.RaiseLoadModuleCompleted(moduleInfo, null);
                }
            }
            catch (Exception ex)
            {
                this.RaiseLoadModuleCompleted(moduleInfo, ex);
            }
        }

        private bool IsSuccessfullyDownloaded(Uri uri)
        {
            lock (this.downloadedUris)
            {
                return this.downloadedUris.Contains(uri);
            }
        }

        private void RecordDownloadSuccess(Uri uri)
        {
            lock (this.downloadedUris)
            {
                this.downloadedUris.Add(uri);
            }
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <remarks>Calls <see cref="Dispose(bool)"/></remarks>.
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the associated <see cref="AssemblyResolver"/>.
        /// </summary>
        /// <param name="disposing">When <see langword="true"/>, it is being called from the Dispose method.</param>
        protected virtual void Dispose(bool disposing)
        {
            IDisposable disposableResolver = this.assemblyResolver as IDisposable;
            if (disposableResolver != null)
            {
                disposableResolver.Dispose();
            }
        }

        #endregion
    }
}
