# Chat App using the SignalR.Couchbase package

This chat application is based on the Microsoft [Real-time chat with SignalR 2](https://docs.microsoft.com/en-us/aspnet/signalr/overview/getting-started/tutorial-getting-started-with-signalr) application and implements the SignalR.Couchbase package to use Couchbase as the messaging backplane.

# Prerequisites
 Setup up a [Couchbase Server cluster.](https://docs.couchbase.com/server/current/getting-started/start-here.html)   
 Create a bucket.  
 Create a primary index on the bucket.  

# Usage
Clone the repo.    
Update the Web.config file with your own Couchbase configuration and run the application.

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
