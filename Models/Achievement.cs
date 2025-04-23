using System;

namespace YouthFit.Models
{
    public class Achievement
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ChallengeId { get; set; }
        public int UserId { get; set; }
        public DateTime DateAchieved { get; set; }

        public Challenge Challenge { get; set; }
        public User User { get; set; }
    }
}
