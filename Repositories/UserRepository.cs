using Microsoft.Data.SqlClient; 
using YouthFit.Models;

namespace YouthFit.Repositories
{
    public class UserRepository
    {
        string connectionstring = "Server=(localdb)\\mssqllocaldb;Database=YouthFitDb;Trusted_Connection=True;";

        public List<User> GetAll()
        {
            using (var connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Users", connection);
                var reader = command.ExecuteReader(); //Line 11 - Line 15 connection
                var users = new List<User>();

                while (reader.Read())
                {
                    var user = new User
                    {
                        Id = reader.GetInt32(0),
                        Username = reader.GetString(1),
                        PasswordHash = reader.GetString(2),
                        Email = reader.GetString(3)
                    };
                    users.Add(user);
                }

                return users;
            }
        }
        
        public User Get(int id)
        {
            using (var connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        return new User
                        {
                            Id = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            PasswordHash = reader.GetString(2),
                            Email = reader.GetString(3)
                        };
                    }
                }
            }
            return null;
        }
        public void Add(User user)
        {
            using (var connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                string query = "INSERT INTO Users (Username, PasswordHash, Email) VALUES (@Username, @PasswordHash, @Email)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(User user)
        {
            using (var connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                string query = "UPDATE Users SET Username = @Username, PasswordHash = @PasswordHash, Email = @Email WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Id", user.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                string query = "DELETE FROM Users WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
