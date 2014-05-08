using System.Data;
using System.Data.SqlClient;
using Common.Test;
using NUnit.Framework;

namespace Database.Test
{
    [TestFixture]
    public class CreateQueueProc
    {
        [Test]
        public void DoesStoredProcExist()
        {
            const string storedProcName = "create_queue";
            var commandText =
                string.Format(
                    "SELECT COUNT(*) FROM [sys].[objects] WHERE [type_desc] = 'SQL_STORED_PROCEDURE' AND [name] = @proc_name");
            SqlConnection sqlConnection = DatabaseConnection.CreateSqlConnection();
            sqlConnection.Open();
            var cmd = new SqlCommand(commandText, sqlConnection);

            var pCount = cmd.Parameters.Add("@proc_name", SqlDbType.VarChar);
            pCount.Value = storedProcName;

            var numberOfStoredProcs = cmd.ExecuteScalar();

            Assert.That(numberOfStoredProcs, Is.EqualTo(1));

        }
    }
}
