using Datyche.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Datyche.Data;

namespace Datyche.Controllers
{
    public class PostController : Controller
    {
        private readonly ILogger<PostController> _logger;
        private readonly DatycheContext _db;

        public PostController(ILogger<PostController> logger, DatycheContext db)
        {
            _logger = logger;
            _db = db;
        }

        // GET: Post/
        public async Task<IActionResult> Index()
        {
            var postList = await _db.Posts.ToListAsync();

            return View(postList);
        }

        // GET: Post/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _db.Posts == null)
            {
                return NotFound();
            }

            var post = await _db.Posts.FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            var files = _db.Files.Where(f => f.Post.Id == id).ToList(); // TODO: Make async?
            post.Files = files;

            return View(post);
        }

        // GET: Post/Create
        [Authorize]
        public IActionResult Create() => View();

        // POST: Post/Create
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Post post)
        {
            if (!ModelState.IsValid) return new ForbidResult();

            var claims = ClaimsPrincipal.Current!.Identities.FirstOrDefault()!.Claims.ToList();

            post.Author = Int32.Parse(claims?.FirstOrDefault(x => x.Type.Equals("Id"))?.Value!);
            post.DateCreated = DateTime.Now.ToUniversalTime();

            // TODO: Keep files order
            foreach (var file in HttpContext.Request.Form.Files)
            {
                var fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var fullPath = $"uploads/{fileName}";

                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                Datyche.Models.File fileToAddToDB = new()
                {
                    Path = fileName, // TODO: Rename the "Path" column to something else (EFCore)
                    Post = post
                };
                await _db.Files.AddAsync(fileToAddToDB);
            }

            await _db.Posts.AddAsync(post);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Post/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // if (id == null || _db.Posts == null) return NotFound();

            // var post = await _db.Posts.FindAsync(id);
            // if (post == null) return NotFound();

            return NotFound();
        }

        // POST: Post/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // TODO: Handle DateTime binding
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            // FIXME Implement edit
            return NotFound();
        }

        // GET: Post/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // FIXME Only author can edit or delete own post 
            if (id == null || _db.Posts == null)
            {
                return NotFound();
            }

            var post = await _db.Posts.FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_db.Posts == null)
            {
                return Problem("Entity set 'DatycheContext.Posts'  is null.");
            }

            var files = _db.Files.Where(f => f.Post.Id == id).ToList();
            foreach (var file in files)
            {
                System.IO.File.Delete($"uploads/{file.Path}");
            }

            var post = await _db.Posts.FindAsync(id);
            if (post != null)
            {
                _db.Posts.Remove(post);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Aetenae() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public bool PostExists(int id)
        {
            return _db.Posts.Any(x => x.Id == id);
        }
    }
}