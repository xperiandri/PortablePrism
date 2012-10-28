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

using Microsoft.Practices.Prism.TestSupport;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Microsoft.Practices.Prism.Tests.ViewModel
{
    [TestClass]
    public class PropertySupportFixture
    {
        [TestMethod]
        public virtual void WhenExtractingNameFromAValidPropertyExpression_ThenPropertyNameReturned()
        {
            var propertyName = PropertySupport.ExtractPropertyName(() => this.InstanceProperty);
            Assert.AreEqual("InstanceProperty", propertyName);
        }

        [TestMethod]
        public void WhenExpressionRepresentsAStaticProperty_ThenExceptionThrown()
        {
            Assert.ThrowsException<ArgumentException>(() => PropertySupport.ExtractPropertyName(() => StaticProperty));
        }

        [TestMethod]
        public void WhenExpressionIsNull_ThenAnExceptionIsThrown()
        {
            Assert.ThrowsException<ArgumentNullException>(() => PropertySupport.ExtractPropertyName<int>(null));
        }

        [TestMethod]
        public void WhenExpressionRepresentsANonMemberAccessExpression_ThenAnExceptionIsThrown()
        {
            Assert.ThrowsException<ArgumentException>(
                () => PropertySupport.ExtractPropertyName(() => this.GetHashCode())
                );
        }

        [TestMethod]
        public void WhenExpressionRepresentsANonPropertyMemberAccessExpression_ThenAnExceptionIsThrown()
        {
            Assert.ThrowsException<ArgumentException>(() => PropertySupport.ExtractPropertyName(() => this.InstanceField));
        }

        [TestMethod]
        [Ignore]    // cannot build the expression
        public void WhenExpressionRepresentsAPropertyWithNoGetMethod_ThenAnExceptionIsThrown()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                       PropertySupport.ExtractPropertyName(
                               Expression.Lambda<Func<int>>(
                                   Expression.MakeMemberAccess(
                                       null,
                                       typeof(PropertySupportFixture).GetRuntimeProperty("SetOnlyStaticProperty")))));
        }

        public static int StaticProperty { get; set; }

        public int InstanceProperty { get; set; }

        public int InstanceField;

        public static int SetOnlyStaticProperty { set { } }
    }
}