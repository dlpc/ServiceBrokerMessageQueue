@echo off
cls
 rem".nuget\NuGet.exe" "Install" "Fake" "-OutputDirectory" "packages" "-ExcludeVersion"
"packages\FAKE\tools\Fake.exe" build.fsx  %1
