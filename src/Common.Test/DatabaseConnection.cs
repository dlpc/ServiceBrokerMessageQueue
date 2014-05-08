using System;
using System.Data.SqlClient;

namespace Common.Test
{
    public class DatabaseConnection
    {
        public static SqlConnection CreateSqlConnection()
        {
            const string server = @".\SQLI03";
            const string database = @"Test_SMO_Database";
            const string connectionStringTemplate = @"Server={0};Database={1};Trusted_Connection=True;";
            string connectionString = String.Format(connectionStringTemplate, server, database);
            var connection = new SqlConnection(connectionString);
            return connection;
        }
    }
}
