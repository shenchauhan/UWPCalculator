using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculation
{
    public class CalculationHistory
    {
        private const string connectionString = "Data Source=SHEN-SURFACE;Initial Catalog=Calculator;Integrated Security=False;User ID=Shen;Password=P@ssw0rd;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        private readonly StringBuilder stringBuilder = new StringBuilder();

        public void Clear()
        {
            stringBuilder.Clear();
        }

        public void AddToCalculation(string calculation)
        {
            stringBuilder.Append(calculation);
        }

        public void AddToHistory(double value)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (var sqlCommand = new SqlCommand($"INSERT INTO[dbo].[History] ([Calculation] ,[Answer]) VALUES ('{stringBuilder.ToString()}', '{value}')", sqlConnection))
                {
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }

            Clear();
        }

        public double FetchFromHistory(string calculation)
        {
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

        public List<string> FetchEntireHistory()
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

        public void ClearHistory()
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
