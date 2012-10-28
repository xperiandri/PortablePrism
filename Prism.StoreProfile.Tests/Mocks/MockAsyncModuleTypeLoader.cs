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
using System.Threading;
using Microsoft.Practices.Prism.Modularity;
using System.Threading.Tasks;

namespace Microsoft.Practices.Prism.Tests.Mocks
{
    public class MockAsyncModuleTypeLoader : IModuleTypeLoader
    {
        private ManualResetEvent callbackEvent;

        public MockAsyncModuleTypeLoader(ManualResetEvent callbackEvent)
        {
            this.callbackEvent = callbackEvent;
        }

        public int SleepTimeOut { get; set; }

        public Exception CallbackArgumentError { get; set; }

        public bool CanLoadModuleType(ModuleInfo moduleInfo)
        {
            return true;
        }

        public void LoadModuleType(ModuleInfo moduleInfo)
        {
            Task.Factory.StartNew(async() =>
            {
                await Task.Delay(SleepTimeOut);

                this.RaiseLoadModuleCompleted(new LoadModuleCompletedEventArgs(moduleInfo, CallbackArgumentError));
                callbackEvent.Set();
            });
        }

        
        public event EventHandler<ModuleDownloadProgressChangedEventArgs> ModuleDownloadProgressChanged;

        private void RaiseLoadModuleProgressChanged(ModuleDownloadProgressChangedEventArgs e)
        {
            if (this.ModuleDownloadProgressChanged != null)
            {
                this.ModuleDownloadProgressChanged(this, e);
            }
        }

        public event EventHandler<LoadModuleCompletedEventArgs> LoadModuleCompleted;

        private void RaiseLoadModuleCompleted(LoadModuleCompletedEventArgs e)
        {
            if (this.LoadModuleCompleted != null)
            {
                this.LoadModuleCompleted(this, e);
            }
        }
    }
}
