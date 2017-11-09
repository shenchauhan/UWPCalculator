using System.Collections.Generic;
using System.Data.SqlClient;

namespace Calculation
{
    /// <summary>
    /// Where all the *magic* happens to store the history in the SQL database.
    /// </summary>
    public class CalculationHistory
    {
        /// <summary>
        /// Connection string to the database.
        /// </summary>
        private const string connectionString = "Data Source=SHEN-SURFACE;Initial Catalog=Calculator;Integrated Security=False;User ID=Shen;Password=P@ssw0rd;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        /// <summary>
        /// Adding the calculation and value to the SQL Database.
        /// </summary>
        public static void AddToHistory(string calculation, double value)
        {
            if (string.IsNullOrEmpty(calculation))
            {
                return;
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (var sqlCommand = new SqlCommand($"INSERT INTO[dbo].[History] ([Calculation] ,[Answer]) VALUES ('{calculation}', '{value}')", sqlConnection))
                {
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Fetch from the SQL Database.
        /// </summary>
        /// <param name="calculation">The calculation to query.</param>
        /// <returns>The value of the calculation.</returns>
        public static double FetchFromHistory(string calculation)
        {
            if (string.IsNullOrEmpty(calculation))
            {
                return 0d;
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (var sqlCommand = new SqlCommand($"SELECT [Answer] FROM [dbo].[History] WHERE [Calculation] ='{calculation}'", sqlConnection))
                {
                    sqlConnection.Open();
                    var results = sqlCommand.ExecuteScalar();

                    return double.Parse(results.ToString());
                }
            }
        }

        /// <summary>
        /// Fetch the entire history from the SQL database.
        /// </summary>
        /// <returns>A list of the all the items in the history.</returns>
        public static List<string> FetchEntireHistory()
        {
            var list = new List<string>();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (var sqlCommand = new SqlCommand($"SELECT [Calculation] FROM [dbo].[History]", sqlConnection))
                {
                    sqlConnection.Open();
                    var results = sqlCommand.ExecuteReader();
                    while (results.Read())
                    {
                        list.Add(results.GetString(0));
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Deletes all the content from the history.
        /// </summary>
        public static void ClearHistory()
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (var sqlCommand = new SqlCommand($"DELETE FROM [dbo].[History]", sqlConnection))
                {
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
