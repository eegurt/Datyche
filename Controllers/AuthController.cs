using Datyche.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Datyche.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        public IActionResult Login()
        {
            return View();
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