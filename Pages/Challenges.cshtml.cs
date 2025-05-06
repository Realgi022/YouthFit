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

            // Set the UserId for the new challenge
            NewChallenge.UserId = userId;

            // Add the new challenge to the database
            _repository.Add(NewChallenge);

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
