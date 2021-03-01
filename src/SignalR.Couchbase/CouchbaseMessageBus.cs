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

using Couchbase;
using Couchbase.KeyValue;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.AspNet.SignalR.Tracing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SignalR.Couchbase
{
    public class CouchbaseMessageBus : ScaleoutMessageBus
    {
        private readonly CouchbaseScaleoutConfiguration _config;
        private readonly TraceSource _trace;

        private bool _connectionReady;

        private ICluster _cluster;
        private IBucket _bucket;
        private ICouchbaseCollection _collection;

        public CouchbaseMessageBus(IDependencyResolver resolver, CouchbaseScaleoutConfiguration configuration)
           : base(resolver, configuration)
        {
            _config = configuration ?? throw new ArgumentNullException("configuration");
            _trace = resolver.Resolve<ITraceManager>()["Signalr." + typeof(CouchbaseMessageBus).Name];

            Task.Run(() => ConnectWithRetry());
        }

        private bool IsReady
        {
            get { return _connectionReady && _cluster != null; }
        }

        internal async Task ConnectWithRetry()
        {
            while (true)
            {
                try
                {
                    await ConnectToCouchbaseAsync();
                    _trace.TraceInformation("Opening stream.");
                    Open(0);

                    await Task.Run(() => ReceivingAsync());

                    break;
                }
                catch (Exception ex)
                {
                    _trace.TraceError("Error connecting to Couchbase - " + ex.GetBaseException());
                }

                await Task.Delay(_config.RetryDelay);
            }
        }

        private async Task ConnectToCouchbaseAsync()
        {
            _trace.TraceInformation("Connecting...");
            _cluster = await Cluster.ConnectAsync(_config.ConnectionString, _config.Username, _config.Password);
            _bucket = await _cluster.BucketAsync(_config.Bucket);
            _collection = _bucket.DefaultCollection();

            _trace.TraceInformation("Connected to Couchbase collection - ", _collection.Name);
        }

        private async Task ReceivingAsync()
        {
            try
            {
                while (true)
                {
                    var query = String.Format("SELECT id, status, streamIndex, data FROM {0} WHERE status = 0 ORDER BY id ASC LIMIT 1;", _config.Bucket);
                    var result = await _cluster.QueryAsync<CouchbaseMessage>(query);

                    if (result == null)
                    {
                        Thread.Sleep(TimeSpan.FromMilliseconds(150));
                        continue;
                    }

                    await foreach (var row in result)
                    {
                        try
                        {
                            var _scaleoutmsg = row.ToScaleoutMessage();
                            OnReceived(row.StreamIndex, (ulong)DateTime.Parse(row.Id).Ticks, _scaleoutmsg);
                            row.Status = 1;
                            await _collection.UpsertAsync(row.Id, row);

                        }

                        catch (Exception ex)
                        {
                            _trace.TraceInformation("Error adding message to InProcessBus. Id={0}, Data={1}. Error={2}, Stack={3}",
                            row.Id, row.Data, ex.Message, ex.StackTrace);
                            Debug.WriteLine(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _connectionReady = false;
                Debug.WriteLine(ex.Message);
            }
        }

        protected override async Task Send(int streamIndex, IList<Message> messages)
        {
            _trace.TraceVerbose("Send called with stream index {0}.", streamIndex);

            var _msg = CouchbaseMessage.FromMessages(streamIndex, messages);
            try {
                await _collection.InsertAsync(_msg.Id, _msg);
            }
            catch(CouchbaseException exception)
            {
                _trace.TraceVerbose("Exception: {0}", exception);
            }
            
        }

        protected override void Dispose(bool disposing)
        {
            _trace.TraceInformation(nameof(CouchbaseMessageBus) + " is being disposed");
            if (disposing)
            {
                Shutdown();
            }
            base.Dispose(disposing);
        }

        private void Shutdown()
        {
            _trace.TraceInformation("Shutdown()");

            if (_cluster != null)
            {
                _cluster.Dispose();
            }
        }
    }
}
