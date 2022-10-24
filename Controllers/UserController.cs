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
            var claims = ClaimsPrincipal.Current.Identities.FirstOrDefault().Claims.ToList();

            var id = claims?.FirstOrDefault(x => x.Type.Equals("Id"))?.Value;
            var email = claims?.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Email))?.Value;
            var username = claims?.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name))?.Value;
            var password = claims?.FirstOrDefault(x => x.Type.Equals("Password"))?.Value;

            var userViewModel = new UserViewModel(id, email, username, password);
            return View(userViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}