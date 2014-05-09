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
        public void DoesInitiatorQueueExist()
        {
        }

        [Test]
        public void DoesInitiatorServiceExist()
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required,new TransactionOptions(){IsolationLevel = IsolationLevel.ReadUncommitted}))
            {
                SqlConnection sqlConnection = DatabaseConnection.CreateSqlConnection();
                sqlConnection.Open();

                var cmd = new SqlCommand("message_queue.create_queue", sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter procNameParam = cmd.Parameters.Add("@queue_name", SqlDbType.NVarChar);
                procNameParam.Value = "test_queue5";

                cmd.ExecuteNonQuery();

                var queueNumber = CheckSysObjectExists("message_queue", "test_queue5_initiator", "SERVICE_QUEUE");
                Assert.That(queueNumber, Is.EqualTo(1));
                
            }
        }

        [Test]
        public void DoesTargetQueueExist()
        {
        }

        [Test]
        public void DoesTargetServiceExist()
        {
        }

        [Test]
        public void Does_CreateQueue_StoredProcExist()
        {
            const string schemaName = "message_queue";
            var numberOfStoredProcs = CheckSysObjectExists(schemaName, StoredProcName, "SQL_STORED_PROCEDURE");

            Assert.That(numberOfStoredProcs, Is.EqualTo(1));
        }
        
        private static object CheckSysObjectExists(string schemaName, string objectName, string type)
        {
            const string commandText = @"
                SELECT COUNT(*) FROM [sys].[objects] inner join sys.schemas 
                on sys.objects.schema_id = sys.schemas.schema_id
                where  [sys].[objects].[type_desc] = @object_type AND [sys].[schemas].[name] = @schema_name AND [sys].[objects].[name] =
                @proc_name";
            SqlConnection sqlConnection = DatabaseConnection.CreateSqlConnection();
            sqlConnection.Open();
            var cmd = new SqlCommand(commandText, sqlConnection);

            SqlParameter objectNameParam = cmd.Parameters.Add("@proc_name", SqlDbType.VarChar);
            objectNameParam.Value = objectName;

            SqlParameter objectTypeParam = cmd.Parameters.Add("@object_type", SqlDbType.VarChar);
            objectTypeParam.Value = type;

            SqlParameter schemaNameParam = cmd.Parameters.Add("@schema_name", SqlDbType.VarChar);
            schemaNameParam.Value = schemaName;

            object numberOfStoredProcs = cmd.ExecuteScalar();
            return numberOfStoredProcs;
        }
    }
}