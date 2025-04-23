using Microsoft.Data.SqlClient;
using YouthFit.Models;

namespace YouthFit.Repositories
{
    public class AchievementRepository
    {
        string connectionString = "Server=(localdb)\\mssqllocaldb;Database=YouthFitDb;Trusted_Connection=True;";

        public List<Achievement> GetAll()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Achievements", connection);
                var reader = command.ExecuteReader();

                var achievements = new List<Achievement>();

                while (reader.Read())
                {
                    var achievement = new Achievement
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Description = reader.GetString(2),
                        ChallengeId = reader.GetInt32(3),
                        UserId = reader.GetInt32(4),
                        DateAchieved = reader.GetDateTime(5)
                    };

                    // Manually loading related entities (Challenge and User)
                    achievement.Challenge = GetChallengeById(achievement.ChallengeId);
                    achievement.User = GetUserById(achievement.UserId);

                    achievements.Add(achievement);
                }

                return achievements;
            }
        }

        public void Add(Achievement achievement)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Achievements (Title, Description, ChallengeId, UserId, DateAchieved) VALUES (@Title, @Description, @ChallengeId, @UserId, @DateAchieved)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", achievement.Title);
                    command.Parameters.AddWithValue("@Description", achievement.Description);
                    command.Parameters.AddWithValue("@ChallengeId", achievement.ChallengeId);
                    command.Parameters.AddWithValue("@UserId", achievement.UserId);
                    command.Parameters.AddWithValue("@DateAchieved", achievement.DateAchieved);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(Achievement achievement)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Achievements SET Title = @Title, Description = @Description, ChallengeId = @ChallengeId, UserId = @UserId, DateAchieved = @DateAchieved WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", achievement.Id);
                    command.Parameters.AddWithValue("@Title", achievement.Title);
                    command.Parameters.AddWithValue("@Description", achievement.Description);
                    command.Parameters.AddWithValue("@ChallengeId", achievement.ChallengeId);
                    command.Parameters.AddWithValue("@UserId", achievement.UserId);
                    command.Parameters.AddWithValue("@DateAchieved", achievement.DateAchieved);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Achievements WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public Achievement Get(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Achievements WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        var achievement = new Achievement
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Description = reader.GetString(2),
                            ChallengeId = reader.GetInt32(3),
                            UserId = reader.GetInt32(4),
                            DateAchieved = reader.GetDateTime(5)
                        };

                        // Manually loading related entities (Challenge and User)
                        achievement.Challenge = GetChallengeById(achievement.ChallengeId);
                        achievement.User = GetUserById(achievement.UserId);

                        return achievement;
                    }

                    return null;
                }
            }
        }

        private Challenge GetChallengeById(int challengeId)
        {
            // Fetch challenge by Id
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Challenges WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", challengeId);
                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        return new Challenge
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Description = reader.GetString(2)
                        };
                    }
                }
            }
            return null;
        }

        private User GetUserById(int userId)
        {
            // Fetch user by Id
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", userId);
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
    }
}
