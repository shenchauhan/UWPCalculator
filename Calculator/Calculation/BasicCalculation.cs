using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculation
{
    public class BasicCalculation
    {
        private const string connectionString = "Data Source=SHEN-HOME;Initial Catalog=Calculator;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public double Add(double x, double y)
        {
            return x + y;
        }

        public double Substract(double x, double y)
        {
            return x - y;
        }

        public double Divide(double x, double y)
        {
            return x / y;
        }

        public double Multiply(double x, double y)
        {
            return x * y;
        }

        public void AddToHistory(string calculation, double value)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (var sqlCommand = new SqlCommand($"INSERT INTO[dbo].[History] ([Calculation] ,[Answer]) VALUES ('{calculation}', '{value}')", sqlConnection))
                {
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
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
    }
}
