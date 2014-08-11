// include Fake lib
#r @"packages/FAKE/tools/FakeLib.dll"
#r @"packages/FAKE/tools/Fake.SQL.dll"

#r "System.Management.Automation"

open Fake
open Fake.SQL
open System.Management.Automation

// Properties
let buildDir = "./build/"
let SQLServer = @".\SQLI03"
let SQLDatabase = @"SBMQ_Dev"
let ConnectionString = sprintf "Server=%s;Database=%s;Trusted_Connection=True;" SQLServer SQLDatabase 
 
// Default target
Target "Default" (fun _ ->
    trace "Executing Default Target"
    
)

Target "CreateSchema" (fun _ ->
    trace "Creating Schema"
    Fake.SQL.SqlServer.RunScriptsFromDirectory ConnectionString ".\src\Database\Schema"
)


Target "CreateStoredProcedures" (fun _ ->
    trace "Creating Stored Procedures"
    Fake.SQL.SqlServer.RunScriptsFromDirectory ConnectionString ".\src\Database\Procs"
)


Target "DropAndCreateDatabase" (fun _ ->
    trace "Deleting database"
    Fake.SQL.SqlServer.DropAndCreateDatabase  ConnectionString
)


"DropAndCreateDatabase"
==> "CreateSchema"
==> "CreateStoredProcedures"
==> "Default"

// start build
RunTargetOrDefault "Default"