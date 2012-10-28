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
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Microsoft.Practices.Prism.Tests
{
    [TestClass]
    public class UriQueryFixture
    {
        [TestMethod]
        public void CanParseEmptyQuery()
        {
            var query = new UriQuery("");

            Assert.AreEqual(0, query.Count());
        }

        [TestMethod]
        public void CanParseQuery()
        {
            var query = new UriQuery("?a=b&c=d");

            Assert.AreEqual(2, query.Count());
            Assert.AreEqual("b", query["a"]);
            Assert.AreEqual("d", query["c"]);
        }

        [TestMethod]
        public void FlattensEmptyQueryToEmptyString()
        {
            var query = new UriQuery();

            Assert.AreEqual("", query.ToString());
        }

        [TestMethod]
        public void FlattensQueryWithSingleElement()
        {
            var query = new UriQuery();
            query.Add("a", "b");

            Assert.AreEqual("?a=b", query.ToString());
        }

        [TestMethod]
        public void FlattensQueryWithMultipleElements()
        {
            var query = new UriQuery();
            query.Add("a", "b");
            query.Add("b", "c");

            Assert.AreEqual("?a=b&b=c", query.ToString());
        }

        [TestMethod]
        public void EscapesQueryElements()
        {
            var query = new UriQuery();
            query.Add("a?", "b@");

            Assert.AreEqual("?a%3F=b%40", query.ToString());
        }

        [TestMethod]
        public void UnescapesQueryElements()
        {
            var query = new UriQuery("?a%3F=b%40");

            Assert.AreEqual(1, query.Count());
            Assert.AreEqual("b@", query["a?"]);
        }
    }
}
