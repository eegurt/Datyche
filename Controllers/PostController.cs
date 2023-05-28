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

            await _db.Posts.AddAsync(post);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Post/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _db.Posts == null) return NotFound();

            var post = await _db.Posts.FindAsync(id);
            if (post == null) return NotFound();

            return View(post);
        }

        // POST: Post/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // TODO: Include Files
        // TODO: Handle DateTime binding
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id", "Title", "Description")] Post post)
        {
            if (id != post.Id) return NotFound();

            if (!ModelState.IsValid) return BadRequest("Not a valid data");

            var existingPost = await _db.Posts.FindAsync(id);
            if (existingPost == null) return NotFound();

            existingPost.Title = post.Title;
            existingPost.Description = post.Description;

            await _db.SaveChangesAsync();

            // TODO: Concurrency
            // try {
            //     _db.Update(post);
            //     await _db.SaveChangesAsync();
            // }
            // catch (DbUpdateConcurrencyException) {
            //     if (!PostExists(post.Id)) return NotFound();
            //     throw;
            // }

            return RedirectToAction(nameof(Index));
        }

        // GET: Post/Delete/5
        public async Task<IActionResult> Delete(int? id)
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