using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace YouthFit.Pages
{
    public class AchievementsModel : PageModel
    {
        public void OnGet()
        {
            ViewData["CurrentPage"] = "/Achievements";
        }
    }
}
