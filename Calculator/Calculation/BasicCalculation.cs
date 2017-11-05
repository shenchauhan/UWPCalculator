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
            using (SqlConnection sqlConnection = new SqlConnection())
            {
                var sqlCommand = new SqlCommand($"INSERT INTO[dbo].[History] ([Calculation] ,[Answer]) VALUES ({calculation}, {value})");
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
            }
        }

        public double FetchFromHistory(string calculation)
        {
            using (SqlConnection sqlConnection = new SqlConnection())
            {
                var sqlCommand = new SqlCommand($"SELECT [ID], [Calculation], [Answer] FROM [dbo].[History] WHERE [ID] ='{calculation}'");
                sqlConnection.Open();
                var results = sqlCommand.ExecuteScalar();

                return (double)results;
            }
        }
    }
}
