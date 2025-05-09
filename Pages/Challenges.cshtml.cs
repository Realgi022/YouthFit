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

        [BindProperty]
        public Challenge NewChallenge { get; set; }

        public List<Challenge> Challenges { get; set; }

        public IActionResult OnGet()
        {
            ViewData["CurrentPage"] = "/Challenges";
            ViewData["BodyClass"] = "page-background";

            // Fetch all challenges
            Challenges = _repository.GetAll().OrderByDescending(c => c.Deadline).ToList();

            return Page();
        }

        public IActionResult OnPost()
        {
            int userId = GetCurrentUserId();

            // Set the UserId and Status for the new challenge
            NewChallenge.UserId = userId;
            NewChallenge.Status = Status.InProgress;  // Default Status

            // Add the new challenge to the database
            _repository.Add(NewChallenge);

            return RedirectToPage();
        }

        public IActionResult OnPostChangeStatus(int challengeId, Status newStatus)
        {
            // Update the status in the database
            _repository.UpdateStatus(challengeId, newStatus);

            // After the status is updated, redirect back to the page
            return RedirectToPage();
        }


        private int GetCurrentUserId()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (idClaim == null)
                throw new InvalidOperationException("User is not logged in.");

            return int.Parse(idClaim.Value);
        }
    }
}
