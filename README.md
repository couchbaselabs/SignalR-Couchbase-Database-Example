# SignalR.Couchbase

Messaging backplane implementation using Couchbase as the backing store. Messages sent by clients from one server are distributed to other clients on other servers connected to the backplane.

| Tool                                             | Latest Version     |
|--------------------------------------------------|--------------------|
| SignalR.Couchbase                               | 3-1-2021 (v0.1.0.0)|


# Why Couchbase and SignalR?
Using Couchbase as a messaging backplane for SignalR allows distribution of messages across clients connected across multiple web servers. With Cross Data Center Replication (XDCR), these servers can be connected to Couchbase clusters in multiple locations. The data replicated between these clusters enables these messages to propagate across multiple servers to their connected clients in real-time. 

# Sample
This repository includes a sample chat application that uses Couchbase as a messaging backplane.

# Prerequisites
To run the chat application, you will first need:   
Visual Studio 2019  
Couchbase Server 6.6 (To run multiple chat app instances across a single cluster)  
Two or more Couchbase Clusters with XDCR enabled (To run multiple chat app instances across multiple clusters)   
ASP.NET

In the future, we would like to port this to be compatible with VSCode out of the box.

# Usage
Import the Couchbase.SignalR package in the Startup.cs file of your .NET Application.  
Import Microsoft.AspNet.SignalR NuGet package to your application.  

```c#
using Microsoft.AspNet.SignalR;
using SignalR.Couchbase;
using System.Configuration;

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
```

Add the configuration settings for your Couchbase connection settings in the Web.config file.

```xml
<configuration>
	<appSettings>
		<add key="connectionString" value="couchbase://localhost" />
		<add key="bucket" value="signalr" />
		<add key="username" value="admin" />
		<add key="password" value="password" />
	</appSettings>
 </configuration>

```

# Load Testing the Sample App
The load tester for the sample chat app is based on [SignalR Tester.](https://github.com/emtecinc/signalr-tester)
### Usage
Build the project in the /load folder and follow the steps to run the Agent [here](https://github.com/emtecinc/signalr-tester#how-to-use-the-tool)


# Creating a Nuget Package
To create a Nuget package of the SignalR.Couchbase library the following command from src\SignalR.Couchbase\
```cmd
   nuget pack SignalR.Couchbase.csproj -IncludeReferencedProjects
```

# Contributions
Contributions gladly accepted via PRs.

# License
Apache License 2.0
