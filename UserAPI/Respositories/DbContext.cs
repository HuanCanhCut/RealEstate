using System.Data;
using MySql.Data.MySqlClient;

namespace UserAPI.Repositories
{
    public class DbContext
    {
        private string connectionString { get; set; }

        public DbContext()
        {
            connectionString = $"Server=localhost;Port={Environment.GetEnvironmentVariable("DB_PORT")!};Database={Environment.GetEnvironmentVariable("DB_NAME")!};User Id={Environment.GetEnvironmentVariable("DB_USER")!};Password={Environment.GetEnvironmentVariable("DB_PASSWORD")!};";
        }

        public DataTable ExecuteQuery(string query)
        {
            DataTable data = new();

            try
            {
                using MySqlConnection connection = new(connectionString);
                {
                    connection.Open();

                    MySqlCommand command = new(query, connection);

                    using MySqlDataAdapter adapter = new(command);

                    adapter.Fill(data);

                    connection.Close();
                }
            }
            catch (MySqlException)
            {
                throw;
            }

            return data;
        }

        public int ExecuteNonQuery(string query)
        {
            int rowsAffected = 0;

            try
            {
                using MySqlConnection connection = new(connectionString);
                {
                    connection.Open();

                    using MySqlTransaction transaction = connection.BeginTransaction();
                    {
                        try
                        {
                            MySqlCommand command = new(query, connection, transaction);
                            rowsAffected = command.ExecuteNonQuery();

                            transaction.Commit();
                        }
                        catch (MySqlException)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (MySqlException)
            {
                throw;
            }

            return rowsAffected;
        }


        public object ExecuteScalar(string query)
        {
            object? result = null;

            try
            {
                using MySqlConnection connection = new(connectionString);
                {
                    connection.Open();

                    MySqlCommand command = new(query, connection);

                    result = command.ExecuteScalar();

                    connection.Close();

                    return result;
                }
            }
            catch (MySqlException)
            {
                throw;
            }

        }
    }
}
