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
using System.Linq;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Microsoft.Practices.Prism.Tests.ViewModel
{
    [TestClass]
    public class ErrorsContainerFixture
    {
        [TestMethod]
        public void WhenCreatingAnInstanceWithANullAction_ThenAnExceptionIsThrown()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new ErrorsContainer<object>(null));
        }

        [TestMethod]
        public void WhenCreatingInstance_ThenHasNoErrors()
        {
            var validation = new ErrorsContainer<string>(pn => { });

            Assert.IsFalse(validation.HasErrors);
            Assert.IsFalse(validation.GetErrors("property1").Any());
        }

        [TestMethod]
        public void WhenSettingErrorsForPropertyWithNoErrors_ThenNotifiesChangesAndHasErrors()
        {
            List<string> validatedProperties = new List<string>();

            var validation = new ErrorsContainer<string>(pn => validatedProperties.Add(pn));

            validation.SetErrors("property1", new[] { "message"});

            Assert.IsTrue(validation.HasErrors);
            Assert.IsTrue(validation.GetErrors("property1").Contains("message"));
            CollectionAssert.AreEqual(new[] { "property1" }, validatedProperties);
        }

        [TestMethod]
        public void WhenSettingNoErrorsForPropertyWithNoErrors_ThenDoesNotNotifyChangesAndHasNoErrors()
        {
            List<string> validatedProperties = new List<string>();

            var validation = new ErrorsContainer<string>(pn => validatedProperties.Add(pn));

            validation.SetErrors("property1", new string[0]);

            Assert.IsFalse(validation.HasErrors);
            Assert.IsFalse(validation.GetErrors("property1").Any());
            Assert.IsFalse(validatedProperties.Any());
        }

        [TestMethod]
        public void WhenSettingErrorsForPropertyWithErrors_ThenNotifiesChangesAndHasErrors()
        {
            List<string> validatedProperties = new List<string>();

            var validation = new ErrorsContainer<string>(pn => validatedProperties.Add(pn));

            validation.SetErrors("property1", new[] { "message" });

            validatedProperties.Clear();

            validation.SetErrors("property1", new[] { "message" });

            Assert.IsTrue(validation.HasErrors);
            Assert.IsTrue(validation.GetErrors("property1").Contains("message"));
            CollectionAssert.AreEqual(new[] { "property1" }, validatedProperties);
        }

        [TestMethod]
        public void WhenSettingNoErrorsForPropertyWithErrors_ThenNotifiesChangesAndHasNoErrors()
        {
            List<string> validatedProperties = new List<string>();

            var validation = new ErrorsContainer<string>(pn => validatedProperties.Add(pn));

            validation.SetErrors("property1", new[] { "message" });

            validatedProperties.Clear();

            validation.SetErrors("property1", new string[0]);

            Assert.IsFalse(validation.HasErrors);
            Assert.IsFalse(validation.GetErrors("property1").Any());
            CollectionAssert.AreEqual(new[] { "property1" }, validatedProperties);
        }


    }
}
