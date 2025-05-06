using Microsoft.Data.SqlClient;
using YouthFit.Models;
using System;
using System.Collections.Generic;

namespace YouthFit.Repositories
{
    public class ChallengeRepository
    {
        string connectionString = "Server=(localdb)\\mssqllocaldb;Database=YouthFitDb;Trusted_Connection=True;";

        // Get all challenges
        public List<Challenge> GetAll()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Challenges", connection);
                var reader = command.ExecuteReader();
                var challenges = new List<Challenge>();

                while (reader.Read())
                {
                    var challenge = new Challenge
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Description = reader.GetString(2),
                        GoalSteps = reader.GetInt32(3),
                        Deadline = reader.GetDateTime(4)
                    };
                    challenges.Add(challenge);
                }

                return challenges;
            }
        }

        // Get a specific challenge by Id
        public Challenge Get(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Challenges WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Challenge
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Description = reader.GetString(2),
                        GoalSteps = reader.GetInt32(3),
                        Deadline = reader.GetDateTime(4)
                    };
                }

                return null;
            }
        }

        // Add a new challenge
        public void Add(Challenge challenge)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Challenges (UserId, Title, Description, GoalSteps, Deadline) " +
                               "VALUES (@UserId, @Title, @Description, @GoalSteps, @Deadline)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", challenge.UserId);
                    command.Parameters.AddWithValue("@Title", challenge.Title);
                    command.Parameters.AddWithValue("@Description", challenge.Description);
                    command.Parameters.AddWithValue("@GoalSteps", challenge.GoalSteps);
                    command.Parameters.AddWithValue("@Deadline", challenge.Deadline);
                    command.ExecuteNonQuery();
                }
            }
        }


        // Update an existing challenge
        public void Update(Challenge challenge)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Challenges SET Title = @Title, Description = @Description, GoalSteps = @GoalSteps, Deadline = @Deadline WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", challenge.Id);
                    command.Parameters.AddWithValue("@Title", challenge.Title);
                    command.Parameters.AddWithValue("@Description", challenge.Description);
                    command.Parameters.AddWithValue("@GoalSteps", challenge.GoalSteps);
                    command.Parameters.AddWithValue("@Deadline", challenge.Deadline);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Delete a challenge
        public void Delete(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Challenges WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
