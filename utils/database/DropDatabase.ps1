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


Function Drop-Database
{
    Param($sqlServerInstance,
        $databaseName)

    Load-Module -name "sqlps"
    $srv  = new-object ("Microsoft.SqlServer.Management.Smo.Server")$sqlServerInstance.ToString()

    $db = New-Object -TypeName Microsoft.SqlServer.Management.Smo.Database -argumentlist $srv, $databaseName
    
    $db = $srv.Databases[$databaseName]
    Write-Host "Droping Database"$databaseName "on" $sqlServerInstance
    $db.Drop()
}


$instance = ".\SQLI03"
$database = "Test_SMO_Database"

Drop-Database $instance $database
