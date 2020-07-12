General comments (many other comments are scattered along the code - read them):

Entity Framework:
*****************
Microsoft.EntityFrameworkCore is a core library. We need to add other dependencies for xpecific DB types we use,
like Microsoft.EntityFrameworkCore.SqlServer.
Microsoft.EntityFrameworkCore.Design is also required.

Installing the command line tool:
dotnet tool install --global dotnet-ef
if errors refering Nuget occures - check C:\Users\ynadler\AppData\Roaming\NuGet\Nuget.config 

In order to enable updating a DB schema when updating entities, we should use migrations:
use command line to manage database, dbcontext, migrations: type "dotnet ef" command to see options.
["dotnet ef dbcontext scaffold" allows creating a DbContext from an existing DB. {"DB First"?}.]
These commands need the DbContext to be associated with a DB provider (specific DB we want to use).
We use LocalDB installation in the Visual Studio (including the graphic tools). Search "Database Provider" to get info regarding 
databases that are supported by EF. If not on Windows, we can still use a SqlServer on a docker {or on remote machine?}.

commands refering a DbContext should be run from the DbContext's containing project's folder and if that's not the startup project 
(which holds the Db options configuration - which specify the the sql server connection string), we should add a flog of: -s pathToStartupProjectProjFile
e.g. dotnet ef migrations add initialCreate -s ..\OdeToFood\OdeToFood.csProj

after creating a migration (will create some files under the Migrations folder in the project), use the "database" command to update the DB with the migration.




