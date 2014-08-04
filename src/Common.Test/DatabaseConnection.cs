using System;
using System.Data.SqlClient;

namespace Common
{
    public class DatabaseConnection
    {
        public static string Database { get; private set; }
        public static string Server { get; private set; }


        public static SqlConnection CreateSqlConnection(string server, string database)
        {
            const string connectionStringTemplate = @"Server={0};Database={1};Trusted_Connection=True;";
            string connectionString = String.Format(connectionStringTemplate, server, database);
            var connection = new SqlConnection(connectionString);
            return connection;
        }

        public static SqlConnection CreateSqlConnection(string connectionString)
        {
            var connection = new SqlConnection(connectionString);
            return connection;
        }


        public static string CreateConnectionString(string server, string database)
        {
            const string connectionStringTemplate = @"Server={0};Database={1};Trusted_Connection=True;";
            string connectionString = String.Format(connectionStringTemplate, server, database);

            return connectionString;

        }
    }
}
