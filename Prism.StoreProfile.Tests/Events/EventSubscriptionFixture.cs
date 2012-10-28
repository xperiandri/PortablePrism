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
using System.Collections.Generic;

namespace Microsoft.Practices.Prism.Tests.Events
{
    [TestClass]
    public class EventSubscriptionFixture
    {
        [TestMethod]
        public void NullTargetInActionThrows()
        {
            var actionDelegateReference = new MockDelegateReference()
            {
                Target = null
            };
            var filterDelegateReference = new MockDelegateReference()
            {
                Target = (Predicate<object>)(arg =>
                {
                    return true;
                })
            };
            Assert.ThrowsException<ArgumentException>(() =>
                new EventSubscription<object>(actionDelegateReference, filterDelegateReference));
        }

        [TestMethod]
        public void DifferentTargetTypeInActionThrows()
        {
            var actionDelegateReference = new MockDelegateReference()
            {
                Target = (Action<int>)delegate { }
            };
            var filterDelegateReference = new MockDelegateReference()
            {
                Target = (Predicate<string>)(arg =>
                {
                    return true;
                })
            };
            Assert.ThrowsException<ArgumentException>(() =>
                new EventSubscription<string>(actionDelegateReference, filterDelegateReference));
        }

        [TestMethod]
        public void NullActionThrows()
        {
            var filterDelegateReference = new MockDelegateReference()
            {
                Target = (Predicate<object>)(arg =>
                {
                    return true;
                })
            };
            Assert.ThrowsException<ArgumentNullException>(() =>
                new EventSubscription<object>(null, filterDelegateReference));
        }

        [TestMethod]
        public void NullTargetInFilterThrows()
        {
            var actionDelegateReference = new MockDelegateReference()
            {
                Target = (Action<object>)delegate { }
            };

            var filterDelegateReference = new MockDelegateReference()
            {
                Target = null
            };
            Assert.ThrowsException<ArgumentException>(() =>
                new EventSubscription<object>(actionDelegateReference, filterDelegateReference));
        }

        [TestMethod]
        public void DifferentTargetTypeInFilterThrows()
        {
            var actionDelegateReference = new MockDelegateReference()
            {
                Target = (Action<string>)delegate { }
            };

            var filterDelegateReference = new MockDelegateReference()
            {
                Target = (Predicate<int>)(arg =>
                {
                    return true;
                })
            };
            Assert.ThrowsException<ArgumentException>(() =>
                new EventSubscription<string>(actionDelegateReference, filterDelegateReference));
        }

        [TestMethod]
        public void NullFilterThrows()
        {
            var actionDelegateReference = new MockDelegateReference()
            {
                Target = (Action<object>)delegate { }
            };

            Assert.ThrowsException<ArgumentNullException>(() =>
                new EventSubscription<object>(actionDelegateReference, null));
        }

        [TestMethod]
        public void CanInitEventSubscription()
        {
            var actionDelegateReference = new MockDelegateReference((Action<object>)delegate { });
            var filterDelegateReference = new MockDelegateReference((Predicate<object>)delegate { return true; });
            var eventSubscription = new EventSubscription<object>(actionDelegateReference, filterDelegateReference);

            var subscriptionToken = new SubscriptionToken(t => { });

            eventSubscription.SubscriptionToken = subscriptionToken;

            Assert.AreSame(actionDelegateReference.Target, eventSubscription.Action);
            Assert.AreSame(filterDelegateReference.Target, eventSubscription.Filter);
            Assert.AreSame(subscriptionToken, eventSubscription.SubscriptionToken);
        }

        [TestMethod]
        public void GetPublishActionReturnsDelegateThatExecutesTheFilterAndThenTheAction()
        {
            var executedDelegates = new List<string>();
            var actionDelegateReference =
                new MockDelegateReference((Action<object>)delegate { executedDelegates.Add("Action"); });

            var filterDelegateReference = new MockDelegateReference((Predicate<object>)delegate
                                                {
                                                    executedDelegates.Add(
                                                        "Filter");
                                                    return true;
                                                });

            var eventSubscription = new EventSubscription<object>(actionDelegateReference, filterDelegateReference);

            var publishAction = eventSubscription.GetExecutionStrategy();

            Assert.IsNotNull(publishAction);

            publishAction.Invoke(null);

            Assert.AreEqual(2, executedDelegates.Count);
            Assert.AreEqual("Filter", executedDelegates[0]);
            Assert.AreEqual("Action", executedDelegates[1]);
        }

        [TestMethod]
        public void GetPublishActionReturnsNullIfActionIsNull()
        {
            var actionDelegateReference = new MockDelegateReference((Action<object>)delegate { });
            var filterDelegateReference = new MockDelegateReference((Predicate<object>)delegate { return true; });

            var eventSubscription = new EventSubscription<object>(actionDelegateReference, filterDelegateReference);

            var publishAction = eventSubscription.GetExecutionStrategy();

            Assert.IsNotNull(publishAction);

            actionDelegateReference.Target = null;

            publishAction = eventSubscription.GetExecutionStrategy();

            Assert.IsNull(publishAction);
        }

        [TestMethod]
        public void GetPublishActionReturnsNullIfFilterIsNull()
        {
            var actionDelegateReference = new MockDelegateReference((Action<object>)delegate { });
            var filterDelegateReference = new MockDelegateReference((Predicate<object>)delegate { return true; });

            var eventSubscription = new EventSubscription<object>(actionDelegateReference, filterDelegateReference);

            var publishAction = eventSubscription.GetExecutionStrategy();

            Assert.IsNotNull(publishAction);

            filterDelegateReference.Target = null;

            publishAction = eventSubscription.GetExecutionStrategy();

            Assert.IsNull(publishAction);
        }

        [TestMethod]
        public void GetPublishActionDoesNotExecuteActionIfFilterReturnsFalse()
        {
            bool actionExecuted = false;
            var actionDelegateReference = new MockDelegateReference()
            {
                Target = (Action<int>)delegate { actionExecuted = true; }
            };
            var filterDelegateReference = new MockDelegateReference((Predicate<int>)delegate
                                                                                            {
                                                                                                return false;
                                                                                            });

            var eventSubscription = new EventSubscription<int>(actionDelegateReference, filterDelegateReference);

            var publishAction = eventSubscription.GetExecutionStrategy();

            publishAction.Invoke(new object[] { null });

            Assert.IsFalse(actionExecuted);
        }

        [TestMethod]
        public void StrategyPassesArgumentToDelegates()
        {
            string passedArgumentToAction = null;
            string passedArgumentToFilter = null;

            var actionDelegateReference = new MockDelegateReference((Action<string>)(obj => passedArgumentToAction = obj));
            var filterDelegateReference = new MockDelegateReference((Predicate<string>)(obj =>
                                                                                            {
                                                                                                passedArgumentToFilter = obj;
                                                                                                return true;
                                                                                            }));

            var eventSubscription = new EventSubscription<string>(actionDelegateReference, filterDelegateReference);
            var publishAction = eventSubscription.GetExecutionStrategy();

            publishAction.Invoke(new[] { "TestString" });

            Assert.AreEqual("TestString", passedArgumentToAction);
            Assert.AreEqual("TestString", passedArgumentToFilter);
        }
    }
}