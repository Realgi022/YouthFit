using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YouthFit.Models;
using YouthFit.Repositories;

namespace YouthFit.Pages
{
    [Authorize]
    public class ChallengesModel : PageModel
    {
        private readonly ChallengeRepository _repository = new ChallengeRepository();
        public string Username { get; set; }
        public bool IsAdmin { get; set; }

        [BindProperty]
        public Challenge NewChallenge { get; set; }

        public List<Challenge> Challenges { get; set; }

        public IActionResult OnGet()
        {
            ViewData["CurrentPage"] = "/Challenges";
            ViewData["BodyClass"] = "page-background";

            Username = User.Identity.Name;
            IsAdmin = User.IsInRole("admin");

            // Get all challenges (no user filter)
            Challenges = _repository.GetAll().OrderByDescending(c => c.Deadline).ToList();

            int completedCount = Challenges.Count(c => c.Status == Status.Completed);
            if (completedCount >= 5)
            {
                TempData["AchievementMessage"] = " Achievement Unlocked: 5 Challenges Completed!";
            }

            return Page();
        }



        public IActionResult OnPost()
        {
            NewChallenge.Status = Status.InProgress;  

            _repository.Add(NewChallenge);

            return RedirectToPage();
        }

        public IActionResult OnPostChangeStatus(int challengeId, Status newStatus)
        {
            _repository.UpdateStatus(challengeId, newStatus);

            return RedirectToPage();
        }
        public IActionResult OnPostDelete(int challengeId)
        {
            _repository.Delete(challengeId);  
            return RedirectToPage();  
        }

    }
}
