using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ScopeLap.DataBaseEngine;
using ScopeLap.Models.DataBaseEngine;

namespace ScopeLap.Controllers
{
    public class CommentariesController : Controller
    {
        private readonly ScopeLapDbContext _context;

        public CommentariesController(ScopeLapDbContext context)
        {
            _context = context;
        }

        // GET: Commentaries
        //public async Task<IActionResult> Index()
        //{
        //    var scopeLapDbContext = _context.Commentaries.Include(c => c.Account).Include(c => c.Post);
        //    return View(await scopeLapDbContext.ToListAsync());
        //}

        //// GET: Commentaries/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var commentary = await _context.Commentaries
        //        .Include(c => c.Account)
        //        .Include(c => c.Post)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (commentary == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(commentary);
        //}

        // GET: Commentaries/Create
        public IActionResult Create(int postId)
        {
            if (_context.Posts.Any(e => e.Id == postId))
            { 
                Commentary commentary = new Commentary() {
                    PostId = postId,
                    Commented = DateTime.MinValue,
                    AccountID = Convert.ToInt32(HttpContext.User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).First())
                };
                return View(commentary);
            }
            
            return RedirectToAction("UserPage", "Account");
        }

        // POST: Commentaries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Commented,CommentText,AccountID,PostId")] Commentary commentary)
        {
            if (ModelState.IsValid)
            {
                if (_context.Posts.Any(e => e.Id == commentary.PostId))
                {
                    commentary.Commented = DateTime.Now;
                    _context.Add(commentary);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Posts", new { id = commentary.PostId });
                }
            }
            return View();
        }

        // GET: Commentaries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentary = await _context.Commentaries.FindAsync(id);
            if (commentary == null)
            {
                return NotFound();
            }

            return View(commentary);
        }

        // POST: Commentaries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Commented,CommentText,AccountID,PostId")] Commentary commentary)
        {
            if (id != commentary.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (commentary.AccountID != Convert.ToInt32(HttpContext.User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).First()))
                    return NotFound();
                try
                {
                    commentary.CommentText = new StringBuilder(
                        $"Обновлено {DateTime.Now.ToShortDateString()}.\n {commentary.CommentText}"
                        ).ToString();
                    _context.Update(commentary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentaryExists(commentary.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Posts", new { id = commentary.PostId });
            }
            return View(commentary);
        }

        //// GET: Commentaries/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var commentary = await _context.Commentaries
        //        .Include(c => c.Account)
        //        .Include(c => c.Post)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (commentary == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(commentary);
        //}

        // POST: Commentaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var commentary = await _context.Commentaries.FindAsync(id);
            var postId = commentary.PostId;
            if (commentary != null)
            {
                _context.Commentaries.Remove(commentary);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Posts", new { id = postId });
        }

        private bool CommentaryExists(int id)
        {
            return _context.Commentaries.Any(e => e.Id == id);
        }
    }
}
