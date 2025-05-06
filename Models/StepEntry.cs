using System;

namespace YouthFit.Models
{
    public class StepEntry
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public int StepCount { get; set; }

        public User User { get; set; }

        public double CaloriesBurned => Math.Round(StepCount * 0.04, 2);

    }
}
