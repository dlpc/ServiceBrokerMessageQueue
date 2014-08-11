// include Fake lib
#r @"packages/FAKE/tools/FakeLib.dll"
#r @"packages/FAKE/tools/Fake.SQL.dll"

#r "System.Management.Automation"

open Fake
open Fake.SQL
open System.Management.Automation

// Properties
let buildDir = "./build/"
let testDlls = !! (buildDir + "*Test*.dll")

// Default target
Target "Default" (fun _ ->
    trace "Executing Default Target"
    
)

Target "BuildApp" (fun _ ->
!! "ServiceBrokerMessageQueue.sln"
|> MSBuildRelease buildDir "Build"
|> Log "AppBuild-Output: "
)

Target "Clean" (fun _ ->
CleanDirs ["./build/"]
)

Target "NUnitTest" (fun _ ->
    printfn "Test dll path %s" buildDir
    
    testDlls
        |> NUnit (fun p -> 
            {p with
                DisableShadowCopy = true; 
                OutputFile = buildDir + "TestResults.xml"})
    
  
)

"Clean"
==> "BuildApp"
==> "NUnitTest"
==> "Default"

// start build
RunTargetOrDefault "Default"