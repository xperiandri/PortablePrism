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
using Microsoft.Practices.Prism.Tests.Mocks;
using Microsoft.Practices.Prism.Events;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Microsoft.Practices.Prism.Tests.Events
{
    [TestClass]
    public class BackgroundEventSubscriptionFixture
    {
        [TestMethod]
        public void ShouldReceiveDelegateOnDifferentThread()
        {
            int calledThreadId = -1;
            ManualResetEvent completeEvent = new ManualResetEvent(false);
            Action<object> action = delegate
            {
                calledThreadId = Environment.CurrentManagedThreadId;
                completeEvent.Set();
            };

            IDelegateReference actionDelegateReference = new MockDelegateReference() { Target = action };
            IDelegateReference filterDelegateReference = new MockDelegateReference() { Target = (Predicate<object>)delegate { return true; } };

            var eventSubscription = new BackgroundEventSubscription<object>(actionDelegateReference, filterDelegateReference);


            var publishAction = eventSubscription.GetExecutionStrategy();

            Assert.IsNotNull(publishAction);

            publishAction.Invoke(null);
#if SILVERLIGHT || NETFX_CORE
            completeEvent.WaitOne(5000);
#else
            completeEvent.WaitOne(5000, false);
#endif
            Assert.AreNotEqual(Environment.CurrentManagedThreadId, calledThreadId);
        }
    }
}
