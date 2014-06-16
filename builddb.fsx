// include Fake lib
#r @"packages/FAKE/tools/FakeLib.dll"
#r @"packages/FAKE/tools/Fake.SQL.dll"

#r "System.Management.Automation"

open Fake
open Fake.SQL
open System.Management.Automation

// Properties
let buildDir = "./build/"
let SQLServer = @".\SQLIO3"
let SQLDatabase = @"SBMQ"

// Default target
Target "Default" (fun _ ->
    trace "Executing Default Target"
    
)

Target "DeleteDatabase" (fun _ ->
    trace "Deleting database"
    
)


Target "CreateDatabase" (fun _ ->
     PowerShell.Create().AddScript(".\utils\database\CreateAndSave.ps1").Invoke()
     |> Seq.iter (printfn "%A")
     )

"CreateDatabase"
==> "Default"

// start build
RunTargetOrDefault "Default"