using Datyche.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MongoDB.Bson;
using MongoDB.Driver;
using BCrypt.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Datyche.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserNamePass input)
        {
            if (!ModelState.IsValid)
            {
                return new StatusCodeResult(400);
            }

            var client = new MongoClient(
                "mongodb+srv://egurt:truge@datyche.yhsit18.mongodb.net/test"
            );
            var database = client.GetDatabase("datyche");
            var collection = database.GetCollection<BsonDocument>("users");

            var filterBuilder = Builders<BsonDocument>.Filter;
            var filter = filterBuilder.Eq("Username", input.Username) & filterBuilder.Eq("Password", BCrypt.Net.BCrypt.HashPassword(input.Password));
            var user = collection.Find(filter);

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, input.Username)};
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var context = HttpContext;
            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return Redirect("/User/Index");
        }

        public async Task<IActionResult> Logout()
        {
            var context = HttpContext;
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Auth/Login");
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

            var client = new MongoClient(
                "mongodb+srv://egurt:truge@datyche.yhsit18.mongodb.net/test"
            );
            var database = client.GetDatabase("datyche");
            var collection = database.GetCollection<BsonDocument>("users");
            collection.InsertOne(user.ToBsonDocument());

            return Json(user);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}