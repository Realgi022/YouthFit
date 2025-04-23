using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YouthFit.Models;
using YouthFit.Repositories;

namespace YouthFit.Pages
{
    public class IndexModel : PageModel
    {
        // Sign‑Up fields
        [BindProperty] public string SignupUsername { get; set; }
        [BindProperty] public string SignupEmail { get; set; }
        [BindProperty] public string SignupPassword { get; set; }
        public string SignupMessage { get; set; }

        // Login fields
        [BindProperty] public string LoginUsername { get; set; }
        [BindProperty] public string LoginPassword { get; set; }
        public string LoginErrorMessage { get; set; }

        private readonly UserRepository _userRepo;

        public IndexModel()
        {
            _userRepo = new UserRepository();
        }

        public void OnGet()
        {
            ViewData["CurrentPage"] = "/Index";

        }

        public IActionResult OnPostSignup()
        {
            if (string.IsNullOrWhiteSpace(SignupUsername) ||
                string.IsNullOrWhiteSpace(SignupEmail) ||
                string.IsNullOrWhiteSpace(SignupPassword))
            {
                SignupMessage = "All fields are required.";
                return Page();
            }

            var user = new User
            {
                Username = SignupUsername,
                Email = SignupEmail,
                PasswordHash = SignupPassword
            };

            _userRepo.Add(user);
            SignupMessage = "Account created! Please log in.";
            SignupUsername = SignupEmail = SignupPassword = null;
            return Page();
        }

        public IActionResult OnPostLogin()
        {
            var user = _userRepo.GetAll()
                        .FirstOrDefault(u =>
                            u.Username == LoginUsername &&
                            u.PasswordHash == LoginPassword);

            if (user == null)
            {
                ViewData["CurrentPage"] = "/Index"; 
                LoginErrorMessage = "Invalid credentials.";
                return Page();
            }

            return RedirectToPage("/Steps");
        }
    }
}
