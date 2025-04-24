using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YouthFit.Models;
using YouthFit.Repositories;

namespace YouthFit.Pages
{
    public class IndexModel : PageModel
    {
        // Sign-Up fields
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

        // ← Make this async and use cookie auth:
        public async Task<IActionResult> OnPostLogin()
        {
            var user = _userRepo.GetAll()
                        .FirstOrDefault(u =>
                            u.Username == LoginUsername &&
                            u.PasswordHash == LoginPassword);

            if (user == null)
            {
                LoginErrorMessage = "Invalid credentials.";
                return Page();
            }

            // 1) Create claims
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name,           user.Username)
            };

            // 2) Create identity & principal
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // 3) Sign in (issues cookie)
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );

            // 4) Redirect to Steps page
            return RedirectToPage("/Steps");
        }
    }
}
