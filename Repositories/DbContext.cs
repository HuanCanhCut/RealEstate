using MySql.Data.MySqlClient;
using System.Data;

namespace RealEstate.Repositories
{
    public class DbContext
    {
        private string connectionString { get; set; }

        public DbContext(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:DefaultConnection"]!;
        }

        public DataTable ExecuteQuery(string query)
        {
            DataTable data = new();

            try
            {
                // trong trường hợp mà khối lệnh bên trong có lỗi thì nó vẫn đóng connection
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


        public string? ExecuteScalar(string query)
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
                }
            }
            catch (MySqlException)
            {
                throw;
            }

            return result?.ToString();
        }
    }
}
