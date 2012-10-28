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
using System.ComponentModel;
using Microsoft.Practices.Prism.Regions;

namespace Microsoft.Practices.Prism.Tests.Mocks
{
    internal class MockRegion : IRegion
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private MockViewsCollection views = new MockViewsCollection();

        public IViewsCollection Views
        {
            get { return views; }
        }

        public IViewsCollection ActiveViews
        {
            get { throw new System.NotImplementedException(); }
        }

        public object Context
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public string Name { get; set; }

        public IRegionManager Add(object view)
        {
            this.views.Add(view);
            return null;
        }

        public IRegionManager Add(object view, string viewName)
        {
            throw new System.NotImplementedException();
        }

        public IRegionManager Add(object view, string viewName, bool createRegionManagerScope)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(object view)
        {
            throw new System.NotImplementedException();
        }

        public void Activate(object view)
        {
            throw new System.NotImplementedException();
        }

        public void Deactivate(object view)
        {
            throw new System.NotImplementedException();
        }

        public object GetView(string viewName)
        {
            throw new System.NotImplementedException();
        }

        public IRegionManager RegionManager { get; set; }

        public IRegionBehaviorCollection Behaviors
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool Navigate(System.Uri source)
        {
            throw new System.NotImplementedException();
        }


        public void RequestNavigate(System.Uri target, System.Action<NavigationResult> navigationCallback)
        {
            throw new System.NotImplementedException();
        }


        public IRegionNavigationService NavigationService
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }


        public System.Comparison<object> SortComparison
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
