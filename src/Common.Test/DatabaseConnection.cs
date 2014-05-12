using System;
using System.Data.SqlClient;

namespace Common.Test
{
    public class DatabaseConnection
    {
        public static SqlConnection CreateSqlConnection(string server, string database)
        {
            const string connectionStringTemplate = @"Server={0};Database={1};Trusted_Connection=True;";
            string connectionString = String.Format(connectionStringTemplate, server, database);
            var connection = new SqlConnection(connectionString);
            return connection;
        }
    }
}
