using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScopeLap.DataBaseEngine;
using ScopeLap.Models;
using ScopeLap.Models.DataBaseEngine;

namespace ScopeLap.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly ScopeLapDbContext _context;

        public PostsController(ScopeLapDbContext context)
        {
            _context = context;
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.UserName = $"@{HttpContext.User.Identity.Name}";
            ViewBag.IdCheck = HttpContext.User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).First();
            ViewBag.AccId = HttpContext.User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).First();
            ViewBag.PostId = id;

            var userBest = await ListSessionViewModel
                .getBestAsync(_context, Convert.ToInt32(HttpContext.User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).First()));
            if (userBest is null)
            {
                ViewBag.FisrtName = HttpContext.User.Claims.Where(x => x.Type == "Name").Select(x => x.Value).First();
                ViewBag.Id = HttpContext.User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).First();
                int usId = Convert.ToInt32(ViewBag.Id);
                var usAcc = _context.Accounts.FirstOrDefault(x => x.Id == usId);
                ViewBag.FullName = $"{usAcc.Firstname} {usAcc.Lastname}";
            }
            else
            {
                ViewBag.Id = userBest.UserId;
                ViewBag.FullName = userBest.Username;
                ViewBag.BestTime = userBest.PrintTime;
                ViewBag.Track = userBest.TrackName;
                ViewBag.Car = userBest.CarName;
            }

            var post = await _context.Posts
                .Include(p => p.Account)
                .Include(p => p.Commentaries.OrderBy(c => c.Commented))
                .ThenInclude(c => c.Account)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewData["Id"] = HttpContext.User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).First();
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountID,Account,Posted,PostText,Content")] Post post)
        {
            
            post.Posted = DateTime.Now;
            
            if (ModelState.IsValid)
            {
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction("UserPage","Account");
            }
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            ViewData["AccId"] = post.AccountID;
            ViewData["Posted"] = post.Posted.ToString();
            if (post == null)
            {
                return NotFound();
            }
            
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Posted,PostText,AccountID")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (post.AccountID != Convert.ToInt32(HttpContext.User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).First()))
                    return NotFound();
                try
                {
                    post.PostText = new StringBuilder($"Обновлено {DateTime.Now.ToShortDateString()}.\n {post.PostText}").ToString();
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("UserPage", "Account");
            }
            
            return View(post);
        }

        //// GET: Posts/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var post = await _context.Posts
        //        .Include(p => p.Account)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (post == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(post);
        //}

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("UserPage", "Account");
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
