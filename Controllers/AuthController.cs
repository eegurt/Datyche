using Datyche.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Datyche.Data;

namespace Datyche.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly DatycheContext _db;

        public AuthController(ILogger<AuthController> logger, DatycheContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (!ModelState.IsValid)
            {
                return Forbid();
            }

            User user;
            try
            {
                user = _db.Users!.Where(u => u.Username!.ToLower() == username.ToLower()).First();
            }
            catch (System.Exception)
            {
                return Unauthorized();
            }

            string hashedPassword = user.Password!;
            if (!BCrypt.Net.BCrypt.Verify(password, hashedPassword))
            {
                return Unauthorized();
            }

            try
            {
                var claims = new List<Claim>
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(ClaimTypes.Name, user.Username!),
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "User");
            }
            catch (System.Exception)
            {
                return RedirectToAction("Login", "Auth");
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(User user)
        {
            if (!ModelState.IsValid) return ValidationProblem();

            if (_db.Users.Any(u => u.Email.ToLower() == user.Email.ToLower())) return Conflict();
            if (_db.Users.Any(u => u.Username.ToLower() == user.Username.ToLower())) return Conflict();

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            await _db.Users!.AddAsync(user);
            await _db.SaveChangesAsync();

            return Json(user);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}