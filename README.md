# TournamentTracker

Learning C# by building a Tournament Tracker that persist on SqlServer or TextFiles as required.

### BackEnd: TrackerLibrary

* .Net Core 
* Dapper
* SqlServer
* TextFiles persistance
* Interface to provide access for the ui/external projects

### FrontEnd: TrackerUI

* .Net Core WinForms 
* Dependency Injections to connect with the Library (external/backend)

## How To Install Locally

* Clone the reporistory. 
* open solution in Visual Studio, and install missing NuGets.
* set the TrackerUI as startup project.
* open the Program.cs 
* on line 19 `GlobalConfig.InitializeConnections(DatabaseType.TextFile);` set the databasetype to Text file and run the app.
* If you would like to use SQL, 
* execute the dbscript/script.sql to create the database on your server.  
* change to `GlobalConfig.InitializeConnections(DatabaseType.Sql);`


Original Youtube series by [Tim Corey](https://www.youtube.com/playlist?list=PLLWMQd6PeGY3t63w-8MMIjIyYS7MsFcCi)  
