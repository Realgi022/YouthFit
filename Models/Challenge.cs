using System;
using System.Collections.Generic;

namespace YouthFit.Models
{
    public class Challenge
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int GoalSteps { get; set; }
        public DateTime Deadline { get; set; }

        public ICollection<Achievement> Achievements { get; set; }
    }
}
