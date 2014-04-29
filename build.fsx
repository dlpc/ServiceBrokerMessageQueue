// include Fake lib
#r @"packages/FAKE/tools/FakeLib.dll"
open Fake

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

"BuildApp"
==> "Default"

// start build
RunTargetOrDefault "Default"