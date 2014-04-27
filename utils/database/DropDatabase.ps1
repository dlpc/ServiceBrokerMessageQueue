#http://www.yunusgulsen.com/2012/01/check-if-powersehll-module-is-already.html
Function Load-Module 
{ 
Param([string]$name) 
if(-not(Get-Module -name $name)) 
    { 
        if(Get-Module -ListAvailable | 
            Where-Object { $_.name -eq $name }) 
        {    
            Import-Module -Name $name
            Write-Host "Importing Module"$name 
            $true 
        }     
        else { 
            Write-Host "Module Not Available - Module Name:"$name 
            
            $false
             } 
    } 
    else { 
        $true
         } 
}


# if (-not(Get-mymodule -name "sqlps"))
# {
#     Write-Host "Sql Server Powershell Not Available"
#     Write-Host "Terminating Script"
#     Exit
# }

Load-Module -name "sqlps"


#Import-Module 'C:\program files\microsoft sql server\110\tools\powershell\modules\sqlps' -DisableNameChecking


$srv  = new-object ("Microsoft.SqlServer.Management.Smo.Server") ".\SQLI03"


#Reference the database and display the date when it was created. 
$db = $srv.Databases["Test_SMO_Database"]
#TODO CreateDatabase function
#TODO DropDatbase function
#Drop the database

$db.Drop()