using System.Collections.Generic;

namespace YouthFit.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }

        public ICollection<StepEntry> StepEntries { get; set; }
        public ICollection<Achievement> Achievements { get; set; }
    }
}
