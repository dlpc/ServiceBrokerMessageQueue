#r @"packages/FAKE/tools/FakeLib.dll"

open Fake


Target "Default" (fun _ ->
    trace "Executing Default Target For Perf Test"
    
)

RunTargetOrDefault "Default"