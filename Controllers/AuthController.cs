using Datyche.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Claims;
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

            var collection = MongoUtils.GetDBUsersCollection();
            var filter = Builders<BsonDocument>.Filter.Eq("Username", input.Username);
            try
            {
                var user = collection.Find(filter);
            }
            catch (System.Exception)
            {
                return new ForbidResult();
            }

            var projection = Builders<BsonDocument>.Projection.Include("Password").Exclude("_id");
            string hashedPassword = collection.Find(filter).Project(projection).FirstOrDefault().Single().Value.ToString();
            bool verifiedPassword = BCrypt.Net.BCrypt.Verify(input.Password, hashedPassword);
            if (!verifiedPassword) 
            {
                return new ForbidResult();
            }

            var userValues = collection.Find(filter).Single().ToDictionary();
            try
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, input.Username),
                    new Claim(ClaimTypes.Email, userValues["Email"].ToString()),
                    new Claim("Id", userValues["Id"].ToString())
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

            var collection = MongoUtils.GetDBUsersCollection();
            collection.InsertOne(user.ToBsonDocument());

            return Json(user);
        }

        public string Test()
        {
            string input = "mark";

            var collection = MongoUtils.GetDBUsersCollection();
            var filter = Builders<BsonDocument>.Filter.Eq("Username", input);
            var userValues = collection.Find(filter).Single().ToDictionary();

            return userValues["Email"].ToString();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}