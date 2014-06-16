@echo off
cls
 rem".nuget\NuGet.exe" "Install" "Fake" "-OutputDirectory" "packages" "-ExcludeVersion"
"packages\FAKE\tools\Fake.exe" builddb.fsx  %1
