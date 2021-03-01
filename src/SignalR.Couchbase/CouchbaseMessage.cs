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
using System.Collections.Generic;

namespace SignalR.Couchbase
{
    public class CouchbaseMessage
    {
        public string Id { get; set; }
        public int StreamIndex { get; set; }
        public byte[] Data { get; set; }

        public int Status { get; set; }

        public static CouchbaseMessage FromMessages(int streamIndex, IList<Message> messages)
        {
            if(messages == null)
            {
                throw new NullReferenceException("messages");
            }
            
            var scaleoutMessage = new ScaleoutMessage(messages);
      
            return new CouchbaseMessage
            {
                Id = DateTime.UtcNow.ToString(),
                StreamIndex = streamIndex,
                Data = scaleoutMessage.ToBytes()
            };
        }

        public ScaleoutMessage ToScaleoutMessage()
        {
            return ScaleoutMessage.FromBytes(Data);
        }
    }
}
