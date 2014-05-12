using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using Common.Test;
using NUnit.Framework;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace Database.Test
{
    [TestFixture]
    public class CreateQueueProcFixture
    {
        private const string StoredProcName = "create_queue";

        [Test]
        public void DoesCreateQueue_CreateInitiatorAndTargetQueuesAndServices()
        {
            using (new TransactionScope(TransactionScopeOption.Required,new TransactionOptions {IsolationLevel = IsolationLevel.ReadUncommitted}))
            {
                var sqlConnection = DatabaseConnection.CreateSqlConnection(@".\SQLI03", @"Test_SMO_Database");
                sqlConnection.Open();

                var cmd = new SqlCommand("message_queue.create_queue", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                var procNameParam = cmd.Parameters.Add("@queue_name", SqlDbType.NVarChar);
                procNameParam.Value = "test_queue5";

                cmd.ExecuteNonQuery();

                var initiatorQueueCount = CheckSysObjectExists("message_queue", "test_queue5_initiator", "SERVICE_QUEUE");
                Assert.That(initiatorQueueCount, Is.True);

                var targetQueueCount = CheckSysObjectExists("message_queue", "test_queue5", "SERVICE_QUEUE");
                Assert.That(targetQueueCount, Is.True);

                var initiatorServiceCount = CheckSysServicesExists("test_queue5_initiator_service");
                Assert.That(initiatorServiceCount, Is.True);

                var targetServiceCount = CheckSysServicesExists("test_queue5_service");
                Assert.That(targetServiceCount, Is.True);
            }
        }

        [Test]
        public void Does_CreateQueue_StoredProcExist()
        {
            const string schemaName = "message_queue";
            var numberOfStoredProcs = CheckSysObjectExists(schemaName, StoredProcName, "SQL_STORED_PROCEDURE");

            Assert.That(numberOfStoredProcs, Is.True);
        }
        
        private static bool CheckSysObjectExists(string schemaName, string objectName, string type)
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

        private static bool CheckSysServicesExists(string serviceName)
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