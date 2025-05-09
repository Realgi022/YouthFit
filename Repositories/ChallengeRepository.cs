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
                        Deadline = reader.GetDateTime(4),
                        Status = (Status)reader.GetInt32(5)  // Assuming Status is stored as an integer in the database
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
                        Deadline = reader.GetDateTime(4),
                        Status = (Status)Enum.Parse(typeof(Status), reader.GetString(5)) // Assuming Status is an enum
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
                string query = "INSERT INTO Challenges (Title, Description, GoalSteps, Deadline, Status) " +
                               "VALUES (@Title, @Description, @GoalSteps, @Deadline, @Status)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", challenge.Title);
                    command.Parameters.AddWithValue("@Description", challenge.Description);
                    command.Parameters.AddWithValue("@GoalSteps", challenge.GoalSteps);
                    command.Parameters.AddWithValue("@Deadline", challenge.Deadline);
                    command.Parameters.AddWithValue("@Status", (int)challenge.Status); // Convert enum to int
                    command.ExecuteNonQuery();
                }
            }
        }



        // Update an existing challenge
        public void UpdateStatus(int challengeId, Status newStatus)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Challenges SET Status = @Status WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", challengeId);
                    command.Parameters.AddWithValue("@Status", (int)newStatus); // Convert Status enum to int
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
