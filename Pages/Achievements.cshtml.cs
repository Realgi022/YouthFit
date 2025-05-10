using Microsoft.AspNetCore.Mvc.RazorPages;
using YouthFit.Models;
using YouthFit.Repositories;

namespace YouthFit.Pages
{
    public class AchievementsModel : PageModel
    {
        public List<Achievement> Achievements { get; set; }

        public void OnGet()
        {
            var repo = new AchievementRepository();
            Achievements = repo.GetAll();
        }
    }
}
