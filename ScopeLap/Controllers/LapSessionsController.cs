using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScopeLap.DataBaseEngine;
using ScopeLap.Models.DataBaseEngine;

namespace ScopeLap.Controllers
{
    [Authorize]
    public class LapSessionsController : Controller
    {
        private readonly ScopeLapDbContext _context;

        public LapSessionsController(ScopeLapDbContext context)
        {
            _context = context;
        }

        // GET: LapSessions
        public async Task<IActionResult> Index()
        {
            var scopeLapDbContext = _context.Sessions.Include(l => l.Account).Include(l => l.Car);
            return View(await scopeLapDbContext.ToListAsync());
        }

        // GET: LapSessions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lapSession = await _context.Sessions
                .Include(l => l.Account)
                .Include(l => l.Car)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lapSession == null)
            {
                return NotFound();
            }

            return View(lapSession);
        }

        // GET: LapSessions/Create
        public IActionResult Create()
        {
            ViewBag.UserName = HttpContext.User.Identity.Name;
            ViewBag.FisrtName = HttpContext.User.Claims.Where(x => x.Type == "Name").Select(x => x.Value).First();
            ViewBag.Id = HttpContext.User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).First();

            var cars = _context.Cars
                .Select(x => new 
                    { 
                        CarId = x.Id,
                        Name = $"{x.Manufacturer} {x.Model} - {x.Description}"
                    }).ToList();

            ViewData["Car"] = new SelectList(cars, "CarId", "Name");
            return View();
        }

        // POST: LapSessions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LapTime,LapNote,AccountId,CarId")] LapSession lapSession)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lapSession);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Email", lapSession.AccountId);
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Manufacturer", lapSession.CarId);
            return View(lapSession);
        }

        // GET: LapSessions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lapSession = await _context.Sessions.FindAsync(id);
            if (lapSession == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Email", lapSession.AccountId);
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Manufacturer", lapSession.CarId);
            return View(lapSession);
        }

        // POST: LapSessions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LapTime,LapNote,AccountId,CarId")] LapSession lapSession)
        {
            if (id != lapSession.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lapSession);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LapSessionExists(lapSession.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Email", lapSession.AccountId);
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Manufacturer", lapSession.CarId);
            return View(lapSession);
        }

        // GET: LapSessions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lapSession = await _context.Sessions
                .Include(l => l.Account)
                .Include(l => l.Car)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lapSession == null)
            {
                return NotFound();
            }

            return View(lapSession);
        }

        // POST: LapSessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lapSession = await _context.Sessions.FindAsync(id);
            if (lapSession != null)
            {
                _context.Sessions.Remove(lapSession);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LapSessionExists(int id)
        {
            return _context.Sessions.Any(e => e.Id == id);
        }
    }
}
