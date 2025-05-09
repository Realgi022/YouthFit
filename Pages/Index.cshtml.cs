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
        [BindProperty] public string SignupUsername { get; set; }
        [BindProperty] public string SignupEmail { get; set; }
        [BindProperty] public string SignupPassword { get; set; }
        public string SignupMessage { get; set; }

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

        public async Task<IActionResult> OnPostLogin()
        {
            if (LoginUsername == "admin" && LoginPassword == "admin123")
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "admin"),
            new Claim(ClaimTypes.Name, "admin"),
            new Claim(ClaimTypes.Role, "admin")
        };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToPage("/Challenges"); 
            }

            var user = _userRepo.GetAll()
                        .FirstOrDefault(u =>
                            u.Username == LoginUsername &&
                            u.PasswordHash == LoginPassword);

            if (user == null)
            {
                LoginErrorMessage = "Invalid credentials.";
                return Page();
            }

            var userClaims = new List<Claim> {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, "user") 
    };

            var userIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);

            return RedirectToPage("/Steps");
        }
    }
}
