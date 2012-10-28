
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
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Tests.Mocks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Linq;

namespace Microsoft.Practices.Prism.Tests.Events
{
    [TestClass]
    public class DispatcherEventSubscriptionFixture
    {
        [TestMethod]
        public void ShouldCallInvokeOnDispatcher()
        {
            DispatcherEventSubscription<object> eventSubscription = null;

            IDelegateReference actionDelegateReference = new MockDelegateReference()
            {
                Target = (Action<object>)(arg =>
                {
                    return;
                })
            };

            IDelegateReference filterDelegateReference = new MockDelegateReference
            {
                Target = (Predicate<object>)(arg => true)
            };
            var mockDispatcher = new MockDispatcher();
            eventSubscription = new DispatcherEventSubscription<object>(actionDelegateReference, filterDelegateReference, mockDispatcher);

            eventSubscription.GetExecutionStrategy().Invoke(new object[0]);

            Assert.IsTrue(mockDispatcher.InvokeCalled);
        }

        [TestMethod]
        public void ShouldPassParametersCorrectly()
        {
            IDelegateReference actionDelegateReference = new MockDelegateReference()
            {
                Target =
                    (Action<object>)(arg1 =>
                    {
                        return;
                    })
            };
            IDelegateReference filterDelegateReference = new MockDelegateReference
            {
                Target = (Predicate<object>)(arg => true)
            };

            var mockDispatcher = new MockDispatcher();
            DispatcherEventSubscription<object> eventSubscription = new DispatcherEventSubscription<object>(actionDelegateReference, filterDelegateReference, mockDispatcher);

            var executionStrategy = eventSubscription.GetExecutionStrategy();
            Assert.IsNotNull(executionStrategy);

            object argument1 = new object();

            executionStrategy.Invoke(new[] { argument1 });

            Assert.AreSame(argument1, mockDispatcher.InvokeArgs.First());
        }
    }

    internal class MockDispatcher : IDispatcherFacade
    {
        public bool InvokeCalled;
        public object[] InvokeArgs;
        private bool hasThreadAccess;

        public void BeginInvoke(Delegate method, params object[] args)
        {
            InvokeCalled = true;
            InvokeArgs = args;
        }

        public bool HasThreadAccess
        {
            get
            {
                return hasThreadAccess;
            }
            set
            {
                hasThreadAccess = value;
            }
        }
    }
}