using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using landingpage.Models;
using Microsoft.AspNetCore.Identity;

namespace landingpage.Controllers
{
    public class AuthController : Controller
    {
        private readonly ProductsdataContext db;

        public AuthController(ProductsdataContext _db)
        {
            db = _db;
        }

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup(User user)
        {
         
            var existingUser = db.Users.FirstOrDefault(a => a.Email == user.Email);

            if (existingUser == null)
            {
              
                var hasher = new PasswordHasher<string>();
                string hashedPassword = hasher.HashPassword(user.Email, user.Password);

                
                user.Password = hashedPassword;

                
                db.Users.Add(user);
                db.SaveChanges();

               
                return RedirectToAction("Login");
            }
            else
            {
                
                ViewBag.Msg = "User already registered. Please login.";

               
                return View();
            }
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string pass)
        {
            bool isAuthenticated = false;
            bool isAdmin = false;
            ClaimsIdentity identity = null;

            if (email == "admin@gmail.com" && pass == "123")
            {
                identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, "Asim"),
                    new Claim(ClaimTypes.Role, "Admin")
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                isAdmin = true;
                isAuthenticated = true;
            }
            else if (email == "user@gmail.com" && pass == "12345")
            {
                identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, "user1"),
                    new Claim(ClaimTypes.Role, "User")
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                isAuthenticated = true;
            }

            if (isAuthenticated)
            {
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                if (isAdmin)
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid email or password";
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }
    }
}
