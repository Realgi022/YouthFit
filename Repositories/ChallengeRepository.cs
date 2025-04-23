using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using YouthFit.Models;

namespace YouthFit.Repositories
{
    public class ChallengeRepository
    {
        private readonly string connectionString =
            "Server=(localdb)\\mssqllocaldb;Database=YouthFitDb;Trusted_Connection=True;";

        public List<Challenge> GetAll()
        {
            var challenges = new List<Challenge>();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(
                    "SELECT Id, Title, Description, GoalSteps, Deadline FROM Challenges", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ch = new Challenge
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Description = reader.GetString(2),
                            GoalSteps = reader.GetInt32(3),
                            Deadline = reader.GetDateTime(4)
                        };
                        ch.Achievements = GetAchievementsByChallengeId(ch.Id);
                        challenges.Add(ch);
                    }
                }
            }
            return challenges;
        }

        public Challenge Get(int id)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(
                    "SELECT Id, Title, Description, GoalSteps, Deadline FROM Challenges WHERE Id = @Id",
                    conn);
                cmd.Parameters.AddWithValue("@Id", id);

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read()) return null;

                    var ch = new Challenge
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Description = reader.GetString(2),
                        GoalSteps = reader.GetInt32(3),
                        Deadline = reader.GetDateTime(4)
                    };
                    ch.Achievements = GetAchievementsByChallengeId(ch.Id);
                    return ch;
                }
            }
        }

        public void Add(Challenge challenge)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(
                    "INSERT INTO Challenges (Title, Description, GoalSteps, Deadline) " +
                    "VALUES (@Title, @Description, @GoalSteps, @Deadline)", conn);

                cmd.Parameters.AddWithValue("@Title", challenge.Title);
                cmd.Parameters.AddWithValue("@Description", challenge.Description);
                cmd.Parameters.AddWithValue("@GoalSteps", challenge.GoalSteps);
                cmd.Parameters.AddWithValue("@Deadline", challenge.Deadline);

                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Challenge challenge)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(
                    "UPDATE Challenges " +
                    "SET Title = @Title, Description = @Description, " +
                        "GoalSteps = @GoalSteps, Deadline = @Deadline " +
                    "WHERE Id = @Id", conn);

                cmd.Parameters.AddWithValue("@Id", challenge.Id);
                cmd.Parameters.AddWithValue("@Title", challenge.Title);
                cmd.Parameters.AddWithValue("@Description", challenge.Description);
                cmd.Parameters.AddWithValue("@GoalSteps", challenge.GoalSteps);
                cmd.Parameters.AddWithValue("@Deadline", challenge.Deadline);

                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM Challenges WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }

        private List<Achievement> GetAchievementsByChallengeId(int challengeId)
        {
            var list = new List<Achievement>();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(
                    "SELECT Id, Title, Description, ChallengeId, UserId, DateAchieved " +
                    "FROM Achievements WHERE ChallengeId = @ChId", conn);
                cmd.Parameters.AddWithValue("@ChId", challengeId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Achievement
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Description = reader.GetString(2),
                            ChallengeId = reader.GetInt32(3),
                            UserId = reader.GetInt32(4),
                            DateAchieved = reader.GetDateTime(5)
                        });
                    }
                }
            }
            return list;
        }
    }
}

