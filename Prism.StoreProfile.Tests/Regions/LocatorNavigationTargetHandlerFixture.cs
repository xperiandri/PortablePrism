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
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.TestSupport;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Moq;

namespace Microsoft.Practices.Prism.Tests.Regions
{
    [TestClass]
    public class LocatorNavigationTargetHandlerFixture
    {
        [TestMethod]
        public void WhenViewExistsAndDoesNotImplementINavigationAware_ThenReturnsView()
        {
            // Arrange

            var serviceLocatorMock = new Mock<IServiceLocator>();

            var region = new Region();

            var view = new TestView();

            region.Add(view);

            var navigationContext = new NavigationContext(null, new Uri(view.GetType().Name, UriKind.Relative));

            var navigationTargetHandler = new TestRegionNavigationContentLoader(serviceLocatorMock.Object);


            // Act

            var returnedView = navigationTargetHandler.LoadContent(region, navigationContext);


            // Assert

            Assert.AreSame(view, returnedView);
        }

        [TestMethod]
        public void WhenRegionHasMultipleViews_ThenViewsWithMatchingTypeNameAreConsidered()
        {
            // Arrange

            var serviceLocatorMock = new Mock<IServiceLocator>();

            var region = new Region();

            var view1 = new TestView();
            var view2 = "view";

            region.Add(view1);
            region.Add(view2);

            var navigationContext = new NavigationContext(null, new Uri(view2.GetType().Name, UriKind.Relative));

            var navigationTargetHandler = new TestRegionNavigationContentLoader(serviceLocatorMock.Object);


            // Act

            var returnedView = navigationTargetHandler.LoadContent(region, navigationContext);


            // Assert

            Assert.AreSame(view2, returnedView);
        }

        [TestMethod]
        public void WhenRegionHasMultipleViews_ThenViewsWithMatchingFullTypeNameAreConsidered()
        {
            // Arrange

            var serviceLocatorMock = new Mock<IServiceLocator>();

            var region = new Region();

            var view1 = new TestView();
            var view2 = "view";

            region.Add(view1);
            region.Add(view2);

            var navigationContext = new NavigationContext(null, new Uri(view2.GetType().FullName, UriKind.Relative));

            var navigationTargetHandler = new TestRegionNavigationContentLoader(serviceLocatorMock.Object);


            // Act

            var returnedView = navigationTargetHandler.LoadContent(region, navigationContext);


            // Assert

            Assert.AreSame(view2, returnedView);
        }

        [TestMethod]
        public void WhenViewExistsAndImplementsINavigationAware_ThenViewIsQueriedForNavigationAndIsReturnedIfAcceptsIt()
        {
            // Arrange

            var serviceLocatorMock = new Mock<IServiceLocator>();

            var region = new Region();

            var viewMock = new Mock<INavigationAware>();
            viewMock
                .Setup(v => v.IsNavigationTarget(It.IsAny<NavigationContext>()))
                .Returns(true)
                .Verifiable();

            region.Add(viewMock.Object);

            var navigationContext = new NavigationContext(null, new Uri(viewMock.Object.GetType().Name, UriKind.Relative));

            var navigationTargetHandler = new TestRegionNavigationContentLoader(serviceLocatorMock.Object);


            // Act

            var returnedView = navigationTargetHandler.LoadContent(region, navigationContext);


            // Assert

            Assert.AreSame(viewMock.Object, returnedView);
            viewMock.VerifyAll();
        }

        [TestMethod]
        public void WhenViewExistsAndHasDataContextThatImplementsINavigationAware_ThenDataContextIsQueriedForNavigationAndIsReturnedIfAcceptsIt()
        {
            // Arrange

            var serviceLocatorMock = new Mock<IServiceLocator>();

            var region = new Region();

            var dataContextMock = new Mock<INavigationAware>();
            dataContextMock
                .Setup(v => v.IsNavigationTarget(It.IsAny<NavigationContext>()))
                .Returns(true)
                .Verifiable();
            var viewMock = new Mock<FrameworkElement>();
            viewMock.Object.DataContext = dataContextMock.Object;

            region.Add(viewMock.Object);

            var navigationContext = new NavigationContext(null, new Uri(viewMock.Object.GetType().Name, UriKind.Relative));

            var navigationTargetHandler = new TestRegionNavigationContentLoader(serviceLocatorMock.Object);


            // Act

            var returnedView = navigationTargetHandler.LoadContent(region, navigationContext);


            // Assert

            Assert.AreSame(viewMock.Object, returnedView);
            dataContextMock.VerifyAll();
        }

        [TestMethod]
        public void WhenNoCurrentMatchingViewExists_ThenReturnsNewlyCreatedInstanceWithServiceLocatorAddedToTheRegion()
        {
            // Arrange

            var serviceLocatorMock = new Mock<IServiceLocator>();

            var region = new Region();

            var view = new TestView();

            serviceLocatorMock
                .Setup(sl => sl.GetInstance<object>(view.GetType().Name))
                .Returns(view);

            var navigationContext = new NavigationContext(null, new Uri(view.GetType().Name, UriKind.Relative));

            var navigationTargetHandler = new TestRegionNavigationContentLoader(serviceLocatorMock.Object);


            // Act

            var returnedView = navigationTargetHandler.LoadContent(region, navigationContext);


            // Assert

            Assert.AreSame(view, returnedView);
            Assert.IsTrue(region.Views.Contains(view));
        }

        [TestMethod]
        public void WhenViewExistsAndImplementsINavigationAware_ThenViewIsQueriedForNavigationAndNewInstanceIsCreatedIfItRejectsIt()
        {
            // Arrange

            var serviceLocatorMock = new Mock<IServiceLocator>();

            var region = new Region();

            var viewMock = new Mock<INavigationAware>();
            viewMock
                .Setup(v => v.IsNavigationTarget(It.IsAny<NavigationContext>()))
                .Returns(false)
                .Verifiable();

            region.Add(viewMock.Object);

            var newView = new TestView();

            serviceLocatorMock
                .Setup(sl => sl.GetInstance<object>(viewMock.Object.GetType().Name))
                .Returns(newView);

            var navigationContext = new NavigationContext(null, new Uri(viewMock.Object.GetType().Name, UriKind.Relative));

            var navigationTargetHandler = new TestRegionNavigationContentLoader(serviceLocatorMock.Object);


            // Act

            var returnedView = navigationTargetHandler.LoadContent(region, navigationContext);


            // Assert

            Assert.AreSame(newView, returnedView);
            Assert.IsTrue(region.Views.Contains(newView));
            viewMock.VerifyAll();
        }

        [TestMethod]
        public void WhenViewExistsAndHasDataContextThatImplementsINavigationAware_ThenDataContextIsQueriedForNavigationAndNewInstanceIsCreatedIfItRejectsIt()
        {
            // Arrange

            var serviceLocatorMock = new Mock<IServiceLocator>();

            var region = new Region();

            var dataContextMock = new Mock<INavigationAware>();
            dataContextMock
                .Setup(v => v.IsNavigationTarget(It.IsAny<NavigationContext>()))
                .Returns(false)
                .Verifiable();
            var viewMock = new Mock<FrameworkElement>();
            viewMock.Object.DataContext = dataContextMock.Object;

            region.Add(viewMock.Object);

            var newView = new TestView();

            serviceLocatorMock
                .Setup(sl => sl.GetInstance<object>(viewMock.Object.GetType().Name))
                .Returns(newView);

            var navigationContext = new NavigationContext(null, new Uri(viewMock.Object.GetType().Name, UriKind.Relative));

            var navigationTargetHandler = new TestRegionNavigationContentLoader(serviceLocatorMock.Object);


            // Act

            var returnedView = navigationTargetHandler.LoadContent(region, navigationContext);


            // Assert

            Assert.AreSame(newView, returnedView);
            Assert.IsTrue(region.Views.Contains(newView));
            dataContextMock.VerifyAll();
        }

        [TestMethod]
        public void WhenViewCannotBeCreated_ThenThrowsAnException()
        {
            var serviceLocatorMock = new Mock<IServiceLocator>();
            serviceLocatorMock
                .Setup(sl => sl.GetInstance<object>(typeof(TestView).Name))
                .Throws<ActivationException>();

            var region = new Region();

            var navigationContext = new NavigationContext(null, new Uri(typeof(TestView).Name, UriKind.Relative));

            var navigationTargetHandler = new TestRegionNavigationContentLoader(serviceLocatorMock.Object);


            // Act

            Assert.ThrowsException<InvalidOperationException>(
                () =>
                {
                    navigationTargetHandler.LoadContent(region, navigationContext);

                });
        }

        [TestMethod]
        public void WhenViewAddedByHandlerDoesNotImplementINavigationAware_ThenReturnsView()
        {
            // Arrange

            var serviceLocatorMock = new Mock<IServiceLocator>();

            var region = new Region();

            var view = new TestView();

            serviceLocatorMock
                .Setup(sl => sl.GetInstance<object>(view.GetType().Name))
                .Returns(view);

            var navigationContext = new NavigationContext(null, new Uri(view.GetType().Name, UriKind.Relative));

            var navigationTargetHandler = new TestRegionNavigationContentLoader(serviceLocatorMock.Object);


            // Act

            var firstReturnedView = navigationTargetHandler.LoadContent(region, navigationContext);
            var secondReturnedView = navigationTargetHandler.LoadContent(region, navigationContext);


            // Assert

            Assert.AreSame(view, firstReturnedView);
            Assert.AreSame(view, secondReturnedView);
            serviceLocatorMock.Verify(sl => sl.GetInstance<object>(view.GetType().Name), Times.Once());
        }

        [TestMethod]
        public void WhenRequestingContentForNullRegion_ThenThrows()
        {
            var serviceLocatorMock = new Mock<IServiceLocator>();

            var navigationContext = new NavigationContext(null, new Uri("/", UriKind.Relative));

            var navigationTargetHandler = new TestRegionNavigationContentLoader(serviceLocatorMock.Object);


            Assert.ThrowsException<ArgumentNullException>(
                () =>
                {
                    navigationTargetHandler.LoadContent(null, navigationContext);

                });
        }

        [TestMethod]
        public void WhenRequestingContentForNullContext_ThenThrows()
        {
            var serviceLocatorMock = new Mock<IServiceLocator>();

            var region = new Region();

            var navigationTargetHandler = new TestRegionNavigationContentLoader(serviceLocatorMock.Object);


            Assert.ThrowsException<ArgumentNullException>(
                () =>
                {
                    navigationTargetHandler.LoadContent(region, null);

                });
        }

        public class TestRegionNavigationContentLoader : RegionNavigationContentLoader
        {
            public TestRegionNavigationContentLoader(IServiceLocator serviceLocator)
                : base(serviceLocator)
            { }
        }

        public class TestView { }
    }
}
