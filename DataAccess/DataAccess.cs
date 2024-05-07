using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TestCrud.Models;

namespace TestCrud.DataAccess
{
    public class DataAccess
    {
        private string connectionString;

        public DataAccess(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public DataTable ExecuteQueryWithResult(string query)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                }
            }

            return dataTable;
        }

        // Method to fetch users from the database
        public List<User> GetUsers(string query)
        {
            DataTable dataTable = ExecuteQueryWithResult(query);
            List<User> userList = new List<User>();

            foreach (DataRow row in dataTable.Rows)
            {
                User user = new User
                {
                    user_id = Convert.ToInt32(row["user_id"]),
                    firstname = row["firstname"].ToString(),
                    lastname = row["lastname"].ToString()
                };

                userList.Add(user);
            }

            return userList;
        }
    }
}
