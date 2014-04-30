// include Fake lib
#r @"packages/FAKE/tools/FakeLib.dll"
#r "System.Management.Automation"
open Fake
open System.Management.Automation

// Properties
let buildDir = "./build/"

// Default target
Target "Default" (fun _ ->
    trace "Executing Default Target"
)

Target "BuildApp" (fun _ ->
!! "ServiceBrokerMessageQueue.sln"
|> MSBuildRelease buildDir "Build"
|> Log "AppBuild-Output: "
)


Target "CreateDatabase" (fun _ ->
     let x= PowerShell.Create().AddScript(".\utils\database\CreateAndSave.ps1").Invoke()
     printf "100"
     )


"CreateDatabase"
==>"BuildApp"
==> "Default"

// start build
RunTargetOrDefault "Default"