#region [ License information          ]

/* ************************************************************
 *
 *   MIT License
 *
 *   Copyright (c) Microsoft Corporation. All rights reserved.
 *
 *   Permission is hereby granted, free of charge, to any person obtaining a copy
 *   of this software and associated documentation files (the "Software"), to deal
 *   in the Software without restriction, including without limitation the rights
 *   *to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *   copies of the Software, and to permit persons to whom the Software is
 *   furnished to do so, subject to the following conditions:
 *
 *   The above copyright notice and this permission notice shall be included in all
 *   copies or substantial portions of the Software.
 *
 *   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *   AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *   LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *   OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *   SOFTWARE
 *
 * ************************************************************/

#endregion

/*
 *  Based on https://docs.microsoft.com/en-us/aspnet/signalr/overview/getting-started/tutorial-getting-started-with-signalr
 */

using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;
using SignalR.Couchbase;
using System.Configuration;

[assembly: OwinStartup(typeof(CouchbaseSignalRChat.Startup))]
namespace CouchbaseSignalRChat
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new CouchbaseScaleoutConfiguration(
                 ConfigurationManager.AppSettings["connectionString"],
                 ConfigurationManager.AppSettings["username"],
                 ConfigurationManager.AppSettings["password"],
                 ConfigurationManager.AppSettings["bucket"]
             );
            GlobalHost.DependencyResolver.UseCouchbase(config);
            app.MapSignalR();
        }
    }
}
