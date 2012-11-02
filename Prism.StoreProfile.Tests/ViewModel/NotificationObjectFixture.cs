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
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Practices.Prism.TestSupport;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Microsoft.Practices.Prism.Tests.ViewModel
{
    [TestClass]
    public class NotificationObjectFixture
    {
        [TestMethod]
        public void WhenNotifyingOnAnInstanceProperty_ThenAnEventIsFired()
        {
            var testObject = new TestNotificationObject();
            var changeTracker = new PropertyChangeTracker(testObject);

            testObject.InstanceProperty = "newValue";

            Assert.IsTrue(changeTracker.ChangedProperties.Contains("InstanceProperty"));
        }

#if SILVERLIGHT || NETFX_CORE
        [TestMethod]
        public void NotificationObjectShouldBeDataContractSerializable()
        {
            var serializer = new DataContractSerializer(typeof(TestNotificationObject));
            var stream = new System.IO.MemoryStream();

            var testObject = new TestNotificationObject();

            serializer.WriteObject(stream, testObject);

            stream.Seek(0, System.IO.SeekOrigin.Begin);

            var reconstitutedObject = serializer.ReadObject(stream) as TestNotificationObject;

            Assert.IsNotNull(reconstitutedObject);
        }
#else
        [TestMethod]
        public void NotificationObjectShouldBeSerializable()
        {
            var serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            var stream = new System.IO.MemoryStream();
            bool invoked = false;

            var testObject = new TestNotificationObject();
            testObject.PropertyChanged += (o, e) => { invoked = true; };

            serializer.Serialize(stream, testObject);

            stream.Seek(0, System.IO.SeekOrigin.Begin);

            var reconstitutedObject = serializer.Deserialize(stream) as TestNotificationObject;

            Assert.IsNotNull(reconstitutedObject);
        }
#endif

        [TestMethod]
        public void NotificationObjectShouldBeXmlSerializable()
        {
            var serializer = new XmlSerializer(typeof(TestNotificationObject));

            var writeStream = new System.IO.StringWriter();

            var testObject = new TestNotificationObject();
            testObject.PropertyChanged += MockHandler;

            serializer.Serialize(writeStream, testObject);


            var readStream = new System.IO.StringReader(writeStream.ToString());
            var reconstitutedObject = serializer.Deserialize(readStream) as TestNotificationObject;

            Assert.IsNotNull(reconstitutedObject);
        }

        private void MockHandler(object o, EventArgs e)
        {
            // does nothing intentionally
        }

#if SILVERLIGHT || NETFX_CORE
        [DataContract]
#else
        [Serializable]
#endif
        public class TestNotificationObject : NotificationObject
        {
            private string instanceValue;

            public string InstanceProperty
            {
                get { return instanceValue; }
                set
                {
                    instanceValue = value;
                    RaisePropertyChanged(() => InstanceProperty);
                }
            }
        }
    }
}
