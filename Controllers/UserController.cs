using Datyche.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Datyche.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {   
            UserViewModel userViewModel = new UserViewModel()
            {
                // Id = User.Claims.Where(c => c.Type == "Id").FirstOrDefault().ToString(),
                // Email = User.Claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault().ToString(),
                Username = User.Identity.Name
            };
            return View(userViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}