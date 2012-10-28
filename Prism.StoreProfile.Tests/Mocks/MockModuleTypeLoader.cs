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
using Microsoft.Practices.Prism.Modularity;

namespace Microsoft.Practices.Prism.Tests.Mocks
{
    public class MockModuleTypeLoader : IModuleTypeLoader
    {        
        public List<ModuleInfo> LoadedModules = new List<ModuleInfo>();
        public bool canLoadModuleTypeReturnValue = true;
        public Exception LoadCompletedError;

        public bool CanLoadModuleType(ModuleInfo moduleInfo)
        {
            return canLoadModuleTypeReturnValue;
        }

        public void LoadModuleType(ModuleInfo moduleInfo)
        {
            this.LoadedModules.Add(moduleInfo);
            this.RaiseLoadModuleCompleted(new LoadModuleCompletedEventArgs(moduleInfo, this.LoadCompletedError));
        }

        public event EventHandler<ModuleDownloadProgressChangedEventArgs> ModuleDownloadProgressChanged;

        public void RaiseLoadModuleProgressChanged(ModuleDownloadProgressChangedEventArgs e)
        {
            if (this.ModuleDownloadProgressChanged != null)
            {
                this.ModuleDownloadProgressChanged(this, e);
            }
        }

        public event EventHandler<LoadModuleCompletedEventArgs> LoadModuleCompleted;

        public void RaiseLoadModuleCompleted(ModuleInfo moduleInfo, Exception error)
        {
            this.RaiseLoadModuleCompleted(new LoadModuleCompletedEventArgs(moduleInfo, error));
        }

        public void RaiseLoadModuleCompleted(LoadModuleCompletedEventArgs e)
        {
            if (this.LoadModuleCompleted != null)
            {                
                this.LoadModuleCompleted(this, e);
            }
        }
    }
}
