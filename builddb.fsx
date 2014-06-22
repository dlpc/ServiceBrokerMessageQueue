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
let SQLDatabase = @"SBMQ"
let ConnectionString = sprintf "Server=%s;Database=%s;Trusted_Connection=True;" SQLServer SQLDatabase 
 
// Default target
Target "Default" (fun _ ->
    trace "Executing Default Target"
    
)

Target "CreateStoredProcedures" (fun _ ->
    trace "Creating Stored Procedures"
)


Target "DropAndCreateDatabase" (fun _ ->
    trace "Deleting database"
    Fake.SQL.SqlServer.DropAndCreateDatabase  ConnectionString
)


Target "CreateDatabase" (fun _ ->
     PowerShell.Create().AddScript(".\utils\database\CreateAndSave.ps1").Invoke()
     |> Seq.iter (printfn "%A")
     )

"DropAndCreateDatabase"
==> "CreateStoredProcedures"
==> "Default"

// start build
RunTargetOrDefault "Default"