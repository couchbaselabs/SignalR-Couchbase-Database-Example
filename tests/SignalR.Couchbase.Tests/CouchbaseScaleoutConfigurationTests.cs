#region [ License information          ]

/* ************************************************************
 *
 *    @author Couchbase <info@couchbase.com>
 *    @copyright 2021 Couchbase, Inc.
 *
 *    Licensed under the Apache License, Version 2.0 (the "License");
 *    you may not use this file except in compliance with the License.
 *    You may obtain a copy of the License at
 *
 *        http://www.apache.org/licenses/LICENSE-2.0
 *
 *    Unless required by applicable law or agreed to in writing, software
 *    distributed under the License is distributed on an "AS IS" BASIS,
 *    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *    See the License for the specific language governing permissions and
 *    limitations under the License.
 *
 * ************************************************************/

#endregion

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SignalR.Couchbase.Tests
{
    [TestClass]
    public class CouchbaseScaleoutConfigurationTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsForNullConnectionString()
        {
            Assert.IsNotNull(new CouchbaseScaleoutConfiguration((string)null, "username", "password", "bucket"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsForEmptyConnectionString()
        {
            Assert.IsNotNull(new CouchbaseScaleoutConfiguration(string.Empty, "username", "password", "bucket"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsForNullUsername()
        {
            Assert.IsNotNull(new CouchbaseScaleoutConfiguration("connectionString", (string)null, "password", "bucket"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsForEmptyUsername()
        {
            Assert.IsNotNull(new CouchbaseScaleoutConfiguration("connectionString", string.Empty, "password", "bucket"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsForNullPassword()
        {
            Assert.IsNotNull(new CouchbaseScaleoutConfiguration("connectionString", "username", (string)null, "bucket"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsForEmptyPassword()
        {
            Assert.IsNotNull(new CouchbaseScaleoutConfiguration("connectionString", "username", string.Empty, "bucket"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsForNullBucket()
        {
            Assert.IsNotNull(new CouchbaseScaleoutConfiguration("connectionString", "username", "password", (string)null));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsForEmptyBucket()
        {
            Assert.IsNotNull(new CouchbaseScaleoutConfiguration("connectionString", "username", "password", string.Empty));
        }
         



    }
}
