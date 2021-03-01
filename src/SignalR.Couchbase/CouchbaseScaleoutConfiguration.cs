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

using Microsoft.AspNet.SignalR.Messaging;
using System;

namespace SignalR.Couchbase
{
    public class CouchbaseScaleoutConfiguration : ScaleoutConfiguration
    {
        public TimeSpan RetryDelay { get; set; }
        public string ConnectionString { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Bucket { get; set; }

        public CouchbaseScaleoutConfiguration(string connectionString, string username, string password, string bucket)
        {
            if (String.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            if (String.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException("username");
            }

            if (String.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password");
            }

            if (String.IsNullOrEmpty(bucket))
            {
                throw new ArgumentNullException("bucket");
            }
            ConnectionString = connectionString;
            Username = username;
            Password = password;
            Bucket = bucket;
            RetryDelay = TimeSpan.FromSeconds(2);
        }
    }
}
