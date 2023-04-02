using Datyche.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MongoDB.Driver;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Datyche.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger) => _logger = logger;

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (!ModelState.IsValid)
            {
                return new ForbidResult();
            }

            var collection = MongoUtils.GetUsersCollection();
            User user;
            try
            {
                user = collection.AsQueryable().Where(u => u.Username.ToLower().Contains(username.ToLower())).Single();
            }
            catch (System.Exception)
            {
                return new NotFoundResult();
            }
            var filter = Builders<User>.Filter.Eq("Username", username);
            var projection = Builders<User>.Projection.Include("Password").Exclude("_id");
            string hashedPassword = user.Password;

            bool verifiedPassword = BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            if (!verifiedPassword)
            {
                return new ForbidResult();
            }

            try
            {
                var claims = new List<Claim>
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
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
        public IActionResult Signup(User user)
        {
            if (!ModelState.IsValid)
            {
                return new StatusCodeResult(400);
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            var collection = MongoUtils.GetUsersCollection();
            collection.InsertOne(user);

            return Json(user);
        }

        public string Test()
        {
            string input = "mark";

            var collection = MongoUtils.GetUsersCollection();
            var filter = Builders<User>.Filter.Eq("Username", input);

            string hashedPassword = collection.AsQueryable().Where(u => u.Username.ToLower().Contains(input.ToLower())).Single().Password;

            return hashedPassword;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}