
. '.\DatabaseUtils.ps1'

$instance = ".\SQLI03"
$database = "Test_SMO_Database"

Create-Database $instance $database
Drop-Database $instance $database