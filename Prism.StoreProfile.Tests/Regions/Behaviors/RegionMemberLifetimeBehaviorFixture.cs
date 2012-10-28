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
using System.Text;
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Regions.Behaviors;
using Microsoft.Practices.Prism.Tests.Mocks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Moq;

namespace Microsoft.Practices.Prism.Tests.Regions.Behaviors
{
    [TestClass]
    public class RegionMemberLifetimeBehaviorFixture 
    {
        protected Region Region { get; set; }
        protected RegionMemberLifetimeBehavior Behavior { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            Arrange();
        }

        protected virtual void Arrange()
        {
            this.Region = new Region();
            this.Behavior = new RegionMemberLifetimeBehavior();
            this.Behavior.Region = this.Region;
            this.Behavior.Attach();
        }

        [TestMethod]
        public void WhenBehaviorAttachedThenReportsIsAttached()
        {
            Assert.IsTrue(Behavior.IsAttached);
        }

        [TestMethod]
        public void WhenIRegionMemberLifetimeItemReturnsKeepAliveFalseRemovesWhenInactive()
        {
            // Arrange
            var regionItemMock = new Mock<IRegionMemberLifetime>();
            regionItemMock.Setup(i => i.KeepAlive).Returns(false);

            Region.Add(regionItemMock.Object);
            Region.Activate(regionItemMock.Object);

            // Act
            Region.Deactivate(regionItemMock.Object);

            // Assert
            Assert.IsFalse(Region.Views.Contains(regionItemMock.Object));
        }

        [TestMethod]
        public void WhenIRegionMemberLifetimeItemReturnsKeepAliveTrueDoesNotRemoveOnDeactivation()
        {
            // Arrange
            var regionItemMock = new Mock<IRegionMemberLifetime>();
            regionItemMock.Setup(i => i.KeepAlive).Returns(true);

            Region.Add(regionItemMock.Object);
            Region.Activate(regionItemMock.Object);

            // Act
            Region.Deactivate(regionItemMock.Object);

            // Assert
            Assert.IsTrue(Region.Views.Contains(regionItemMock.Object));

        }

        [TestMethod]
        public void WhenRegionContainsMultipleMembers_OnlyRemovesThoseDeactivated()
        {
            // Arrange
            var firstMockItem = new Mock<IRegionMemberLifetime>();
            firstMockItem.Setup(i => i.KeepAlive).Returns(true);

            var secondMockItem = new Mock<IRegionMemberLifetime>();
            secondMockItem.Setup(i => i.KeepAlive).Returns(false);

            Region.Add(firstMockItem.Object);
            Region.Activate(firstMockItem.Object);

            Region.Add(secondMockItem.Object);
            Region.Activate(secondMockItem.Object);

            // Act
            Region.Deactivate(secondMockItem.Object);

            // Assert
            Assert.IsTrue(Region.Views.Contains(firstMockItem.Object));
            Assert.IsFalse(Region.Views.Contains(secondMockItem.Object));
        }

        [TestMethod]
        public void WhenMemberNeverActivatedThenIsNotRemovedOnAnothersDeactivation()
        {
            // Arrange
            var firstMockItem = new Mock<IRegionMemberLifetime>();
            firstMockItem.Setup(i => i.KeepAlive).Returns(false);

            var secondMockItem = new Mock<IRegionMemberLifetime>();
            secondMockItem.Setup(i => i.KeepAlive).Returns(false);

            Region.Add(firstMockItem.Object);  // Never activated

            Region.Add(secondMockItem.Object);
            Region.Activate(secondMockItem.Object);

            // Act
            Region.Deactivate(secondMockItem.Object);

            // Assert
            Assert.IsTrue(Region.Views.Contains(firstMockItem.Object));
            Assert.IsFalse(Region.Views.Contains(secondMockItem.Object));
        }

        [TestMethod]
        public virtual void RemovesRegionItemIfDataContextReturnsKeepAliveFalse()
        {
            // Arrange
            var regionItemMock = new Mock<IRegionMemberLifetime>();
            regionItemMock.Setup(i => i.KeepAlive).Returns(false);

            var regionItem = new MockFrameworkElement();
            regionItem.DataContext = regionItemMock.Object;

            Region.Add(regionItem);
            Region.Activate(regionItem);

            // Act
            Region.Deactivate(regionItem);

            // Assert
            Assert.IsFalse(Region.Views.Contains(regionItem));
        }

        [TestMethod]
        public virtual void RemovesOnlyDeactivatedItemsInRegionBasedOnDataContextKeepAlive()
        {
            // Arrange
            var retionItemDataContextToKeepAlive = new Mock<IRegionMemberLifetime>();
            retionItemDataContextToKeepAlive.Setup(i => i.KeepAlive).Returns(true);

            var regionItemToKeepAlive = new MockFrameworkElement();
            regionItemToKeepAlive.DataContext = retionItemDataContextToKeepAlive.Object;
            Region.Add(regionItemToKeepAlive);
            Region.Activate(regionItemToKeepAlive);

            var regionItemMock = new Mock<IRegionMemberLifetime>();
            regionItemMock.Setup(i => i.KeepAlive).Returns(false);

            var regionItem = new MockFrameworkElement();
            regionItem.DataContext = regionItemMock.Object;

            Region.Add(regionItem);
            Region.Activate(regionItem);

            // Act
            Region.Deactivate(regionItem);

            // Assert
            Assert.IsFalse(Region.Views.Contains(regionItem));
            Assert.IsTrue(Region.Views.Contains(regionItemToKeepAlive));
        }

        [TestMethod]
        public virtual void WillRemoveDeactivatedItemIfKeepAliveAttributeFalse()
        {
            // Arrange
            var regionItem = new RegionMemberNotKeptAlive();

            Region.Add(regionItem);
            Region.Activate(regionItem);

            // Act
            Region.Deactivate(regionItem);

            // Assert
            Assert.IsFalse(Region.Views.Contains((object)regionItem));
        }

        [TestMethod]
        public virtual void WillNotRemoveDeactivatedItemIfKeepAliveAttributeTrue()
        {
            // Arrange
            var regionItem = new RegionMemberKeptAlive();

            Region.Add(regionItem);
            Region.Activate(regionItem);

            // Act
            Region.Deactivate(regionItem);

            // Assert
            Assert.IsTrue(Region.Views.Contains((object)regionItem));
        }

        [TestMethod]
        public virtual void WillRemoveDeactivatedItemIfDataContextKeepAliveAttributeFalse()
        {
            // Arrange
            var regionItemDataContext = new RegionMemberNotKeptAlive();
            var regionItem = new MockFrameworkElement() { DataContext = regionItemDataContext };
            Region.Add(regionItem);
            Region.Activate(regionItem);

            // Act
            Region.Deactivate(regionItem);

            // Assert
            Assert.IsFalse(Region.Views.Contains(regionItem));
        }

        [RegionMemberLifetime(KeepAlive = false)]
        public class RegionMemberNotKeptAlive
        {
        }

        [RegionMemberLifetime(KeepAlive = true)]
        public class RegionMemberKeptAlive
        {
        }

        
    }

    [TestClass]
    public class RegionMemberLifetimeBehaviorAgainstSingleActiveRegionFixture
                : RegionMemberLifetimeBehaviorFixture
    {
        protected override void Arrange()
        {
            this.Region = new SingleActiveRegion();
            this.Behavior = new RegionMemberLifetimeBehavior();
            this.Behavior.Region = this.Region;
            this.Behavior.Attach();
        }
    }
}
