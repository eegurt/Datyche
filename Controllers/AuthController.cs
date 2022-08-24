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
                return new ForbidResult();
            }

            var client = new MongoClient("mongodb+srv://egurt:truge@datyche.yhsit18.mongodb.net/test");
            var database = client.GetDatabase("datyche");
            var collection = database.GetCollection<BsonDocument>("users");

            bool verifiedPassword;
            var filter = Builders<BsonDocument>.Filter.Eq("Username", input.Username);
            var projection = Builders<BsonDocument>.Projection.Include("Password").Exclude("_id");
            try
            {
                string hashedPassword = collection.Find(filter).Project(projection).FirstOrDefault().Single().Value.ToString();
                verifiedPassword = BCrypt.Net.BCrypt.Verify(input.Password, hashedPassword);
            }
            catch (System.Exception)
            {
                return new ForbidResult();
            }

            if(!verifiedPassword) return new ForbidResult();

            try
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, input.Username)
                };
                var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "User");
            }
            catch (System.Exception)
            {
                return RedirectToAction("Login", "Auth");
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
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