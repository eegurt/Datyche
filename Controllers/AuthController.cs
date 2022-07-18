using Datyche.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Datyche.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index() // Login
        {
            return View();
        }

        public IActionResult Signup()
        {
            string email = Request.Form["email"];
            string username = Request.Form["username"];
            string password = Request.Form["password"];
            User signedUser = new User(email, username, password);
            
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}