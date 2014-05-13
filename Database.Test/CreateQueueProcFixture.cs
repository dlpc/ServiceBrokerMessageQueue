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

                var initiatorQueueCount = DatabaseVerification.CheckSysObjectExists("message_queue", "test_queue5_initiator", "SERVICE_QUEUE");
                Assert.That(initiatorQueueCount, Is.True);

                var targetQueueCount = DatabaseVerification.CheckSysObjectExists("message_queue", "test_queue5", "SERVICE_QUEUE");
                Assert.That(targetQueueCount, Is.True);

                var initiatorServiceCount = DatabaseVerification.CheckSysServicesExists("test_queue5_initiator_service");
                Assert.That(initiatorServiceCount, Is.True);

                var targetServiceCount = DatabaseVerification.CheckSysServicesExists("test_queue5_service");
                Assert.That(targetServiceCount, Is.True);
            }
        }

        [Test]
        public void Does_CreateQueue_StoredProcExist()
        {
            const string schemaName = "message_queue";
            var numberOfStoredProcs = DatabaseVerification.CheckSysObjectExists(schemaName, StoredProcName, "SQL_STORED_PROCEDURE");

            Assert.That(numberOfStoredProcs, Is.True);
        }
    }
}