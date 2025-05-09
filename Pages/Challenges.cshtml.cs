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

            Challenges = _repository.GetAll().OrderByDescending(c => c.Deadline).ToList();
            Username = User.Identity.Name;
            IsAdmin = User.IsInRole("admin");

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
