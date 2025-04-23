using Microsoft.Data.SqlClient;
using YouthFit.Models;

namespace YouthFit.Repositories
{
    public class StepEntryRepository
    {
        string connectionString = "Server=(localdb)\\mssqllocaldb;Database=YouthFitDb;Trusted_Connection=True;";

        public List<StepEntry> GetAll()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM StepEntries", connection);
                var reader = command.ExecuteReader();
                var stepEntries = new List<StepEntry>();

                while (reader.Read())
                {
                    var stepEntry = new StepEntry
                    {
                        Id = reader.GetInt32(0),
                        UserId = reader.GetInt32(1),
                        Date = reader.GetDateTime(2),
                        StepCount = reader.GetInt32(3)
                    };
                    stepEntries.Add(stepEntry);
                }

                return stepEntries;
            }
        }

        public StepEntry Get(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM StepEntries WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new StepEntry
                    {
                        Id = reader.GetInt32(0),
                        UserId = reader.GetInt32(1),
                        Date = reader.GetDateTime(2),
                        StepCount = reader.GetInt32(3)
                    };
                }

                return null;
            }
        }

        public void Add(StepEntry stepEntry)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO StepEntries (UserId, Date, StepCount) VALUES (@UserId, @Date, @StepCount)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", stepEntry.UserId);
                    command.Parameters.AddWithValue("@Date", stepEntry.Date);
                    command.Parameters.AddWithValue("@StepCount", stepEntry.StepCount);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(StepEntry stepEntry)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE StepEntries SET UserId = @UserId, Date = @Date, StepCount = @StepCount WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", stepEntry.Id);
                    command.Parameters.AddWithValue("@UserId", stepEntry.UserId);
                    command.Parameters.AddWithValue("@Date", stepEntry.Date);
                    command.Parameters.AddWithValue("@StepCount", stepEntry.StepCount);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM StepEntries WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

