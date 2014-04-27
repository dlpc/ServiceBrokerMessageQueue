
Function Get-MyModule 
{ 
Param([string]$name) 
if(-not(Get-Module -name $name)) 
{ 
if(Get-Module -ListAvailable | 
Where-Object { $_.name -eq $name }) 
{ 
Import-Module -Name $name 
$true 
} #end if module available then import 
else { $false } #module not available 
} # end if not module 
else { $true } #module already loaded 
} #end function get-MyModule



if (-not(Get-mymodule -name "sqlps"))
{
    Write-Host "Sql Server Powershell Not Available"
    Write-Host "Terminating Script"
    Exit
}

Import-Module 'C:\program files\microsoft sql server\110\tools\powershell\modules\sqlps' -DisableNameChecking


$srv  = new-object ("Microsoft.SqlServer.Management.Smo.Server") ".\SQLI03"

#Create a new database
$db = New-Object -TypeName Microsoft.SqlServer.Management.Smo.Database -argumentlist $srv, "Test_SMO_Database"
$db.Create()

#Reference the database and display the date when it was created. 
$db = $srv.Databases["Test_SMO_Database"]
$db.CreateDate

#Drop the database

#$db.Drop()