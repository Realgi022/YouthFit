using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YouthFit.Models;
using YouthFit.Repositories;

namespace YouthFit.Pages
{
    [Authorize]   
    public class StepsModel : PageModel
    {
        private readonly StepEntryRepository _repository = new StepEntryRepository();

        [BindProperty]
        public StepEntry NewStep { get; set; }

        public List<StepEntry> StepHistory { get; set; }
        public Dictionary<string, double> MonthlyAverages { get; set; }


        public IActionResult OnGet()
        {
            ViewData["CurrentPage"] = "/Steps";
            ViewData["BodyClass"] = "page-background";

            int userId = GetCurrentUserId();
            StepHistory = _repository
                .GetAll()
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.Date)
                .ToList();

            // Group by year and month
            MonthlyAverages = StepHistory
                .GroupBy(s => s.Date.ToString("yyyy-MM"))
                .ToDictionary(
                    g => g.Key,
                    g => Math.Round(g.Average(s => s.StepCount), 2)
                );

            return Page();
        }


        public IActionResult OnPost()
        {
            int userId = GetCurrentUserId();
            NewStep.UserId = userId;
            _repository.Add(NewStep);
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
