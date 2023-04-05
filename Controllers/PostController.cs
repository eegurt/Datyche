using Datyche.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Diagnostics;
using System.Security.Claims;

namespace Datyche.Controllers
{
    public class PostController : Controller
    {
        private readonly ILogger<PostController> _logger;
        private readonly IMongoDatabase _db;

        public PostController(ILogger<PostController> logger, IMongoDatabase db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public IActionResult Index() => View();

        [HttpGet]
        [Authorize]
        public IActionResult Create() => View();

        [HttpPost]
        [Authorize]
        public IActionResult Create(Post post)
        {
            if (!ModelState.IsValid) return new ForbidResult();

            var claims = ClaimsPrincipal.Current!.Identities.FirstOrDefault()!.Claims.ToList();

            post.Author = new MongoDB.Bson.ObjectId(claims?.FirstOrDefault(x => x.Type.Equals("Id"))?.Value);
            post.Date = DateTime.Now;

            var files = Request.Form.Files;
            var filesCount = files.Count;
            post.Files = new byte[filesCount][];

            for (int i = 0; i < filesCount; i++)
            {
                using (BinaryReader br = new BinaryReader(files[i].OpenReadStream()))
                {
                    byte[] binData = br.ReadBytes((int)files[i].Length);
                    post.Files[i] = binData;
                }
            }
            var collection = _db.GetCollection<Post>("posts");
            collection.InsertOne(post);

            return Json(post);
        }
        public IActionResult Aetenae() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}