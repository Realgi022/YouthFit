using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace YouthFit.Pages
{
    public class StepsModel : PageModel
    {
        public void OnGet()
        {
            ViewData["CurrentPage"] = "/Steps";
            ViewData["BodyClass"] = "page-background";

        }
    }
}
