using System.Data;
using System.Data.SqlClient;

namespace Common
{
    public class DatabaseVerification
    {
        public static bool CheckSysObjectExists(string schemaName, string objectName, string type)
        {
            const string commandText = @"
                SELECT COUNT(*) FROM [sys].[objects] inner join sys.schemas 
                on sys.objects.schema_id = sys.schemas.schema_id
                where  [sys].[objects].[type_desc] = @object_type AND [sys].[schemas].[name] = @schema_name AND [sys].[objects].[name] =
                @proc_name";
            var sqlConnection = DatabaseConnection.CreateSqlConnection(@".\SQLI03", @"Test_SMO_Database");
            sqlConnection.Open();
            var cmd = new SqlCommand(commandText, sqlConnection);

            var objectNameParam = cmd.Parameters.Add("@proc_name", SqlDbType.VarChar);
            objectNameParam.Value = objectName;

            var objectTypeParam = cmd.Parameters.Add("@object_type", SqlDbType.VarChar);
            objectTypeParam.Value = type;

            var schemaNameParam = cmd.Parameters.Add("@schema_name", SqlDbType.VarChar);
            schemaNameParam.Value = schemaName;

            object numberOfStoredProcs = cmd.ExecuteScalar();
            return (int)numberOfStoredProcs ==  1;
        }

        public static bool CheckSysServicesExists(string serviceName)
        {
            const string commandText = @"
                           SELECT COUNT(*)
                            FROM sys.services
                            WHERE name = @service_name
                            ;";

            var sqlConnection = DatabaseConnection.CreateSqlConnection(@".\SQLI03", @"Test_SMO_Database");
            sqlConnection.Open();
            var cmd = new SqlCommand(commandText, sqlConnection);

            var serviceParamName = cmd.Parameters.Add("@service_name", SqlDbType.VarChar);
            serviceParamName.Value = serviceName;

            var numberOfServices = cmd.ExecuteScalar();
            return (int)numberOfServices == 1;
        }
    }
}
