using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScopeLap.DataBaseEngine;
using ScopeLap.Models.DataBaseEngine;

namespace ScopeLap.Controllers
{
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
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Email");
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Manufacturer");
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
