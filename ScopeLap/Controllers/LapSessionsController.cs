using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScopeLap.DataBaseEngine;
using ScopeLap.Models;
using ScopeLap.Models.DataBaseEngine;
using ScopeLap.Tools;
using static System.Collections.Specialized.BitVector32;

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
            ViewBag.UserName = HttpContext.User.Identity.Name;
            ViewBag.FisrtName = HttpContext.User.Claims.Where(x => x.Type == "Name").Select(x => x.Value).First();
            ViewBag.Id = HttpContext.User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).First();
            

            var scopeLapDbContext = _context.Sessions
                .Include(l => l.Account)
                .Include(l => l.Car)
                .Include(l => l.Track);
            
            return View(await scopeLapDbContext.ToListAsync());
        }

        public async Task<IActionResult> MySessions()
        {
            ViewBag.FisrtName = HttpContext.User.Claims.Where(x => x.Type == "Name").Select(x => x.Value).First();
            ViewBag.Id = HttpContext.User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).First();
            ViewBag.UserName = HttpContext.User.Identity.Name;

            int userId = Convert.ToInt32(ViewBag.Id);

            var userSessions = await _context.Sessions.Where(acc => acc.AccountId == userId)
                .Include(l => l.Account)
                .Include(l => l.Car)
                .Include(l => l.Track)
                .Select(
                    session => new ListSessionViewModel
                    {
                        Id = session.Id,
                        Time = session.LapTime,
                        PrintTime = (
                            $"{(session.LapTime / 60000).ToString().PadLeft(2, '0')}" +
                            $":{(session.LapTime % 60000 / 1000).ToString().PadLeft(2, '0')}" +
                            $":{(session.LapTime % 1000).ToString().PadLeft(3, '0')}"
                        ),
                        CarID = session.Car.Id,
                        CarName = $"{session.Car.Manufacturer} {session.Car.Model}",
                        UserId = session.Account.Id,
                        Username = $"{session.Account.Firstname} {session.Account.Lastname}",
                        TrackId = session.Track.Track.Id,
                        TrackConfigId = (int)session.TrackId,
                        TrackName = $"{session.Track.Track.Name}: {session.Track.Name} - {session.Track.Length} м",
                        SessionDate = session.TrackDate
                    }
                )
                .ToListAsync();

            return View(userSessions);
        }

        // GET: LapSessions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
             if (id == null)
             {
                 return NotFound();
             }

             ViewBag.UserName = HttpContext.User.Identity.Name;
             ViewBag.FisrtName = HttpContext.User.Claims.Where(x => x.Type == "Name").Select(x => x.Value).First();
             ViewBag.Id = HttpContext.User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).First();
             ViewBag.IdCheck = HttpContext.User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).First();

            var userSession = await _context.Sessions
                .Include(l => l.Account)
                .Include(l => l.Car)
                .Include(l => l.Track)
                .Select(
                     session => new ListSessionViewModel{
                         Id = session.Id,
                         PrintTime = (
                             $"{(session.LapTime / 60000).ToString().PadLeft(2, '0')}" +
                             $":{(session.LapTime % 60000 / 1000).ToString().PadLeft(2, '0')}" +
                             $":{(session.LapTime % 1000).ToString().PadLeft(3, '0')}"
                         ),
                         CarID = session.Car.Id,
                         CarName = $"{session.Car.Manufacturer} {session.Car.Model}",
                         UserId = session.Account.Id,
                         Username = $"{session.Account.Firstname} {session.Account.Lastname}",
                         TrackId = session.Track.Track.Id,
                         TrackConfigId = (int)session.TrackId,
                         TrackName = $"{session.Track.Track.Name}: {session.Track.Name} - {session.Track.Length} м",
                         SessionDate = session.TrackDate,
                         SessionNote = session.LapNote
                     }
                 )
                .SingleOrDefaultAsync(us => us.Id == id);

             if (userSession == null)
             {
                 return NotFound();
             }

             return View(userSession);


        }

        // GET: LapSessions/Create
        public IActionResult Create()
        {
            ViewBag.UserName = HttpContext.User.Identity.Name;
            ViewBag.FisrtName = HttpContext.User.Claims.Where(x => x.Type == "Name").Select(x => x.Value).First();
            ViewBag.Id = HttpContext.User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).First();
            ViewBag.DefaultDate = DateTime.Today.Date.ToShortDateString();

            var cars = _context.Cars
                .Select(x => new 
                    { 
                        CarId = x.Id,
                        Name = $"{x.Manufacturer} {x.Model} - {x.Description}"
                    }).ToList();

            var tracks = _context.TrackConfigurations.Include(c => c.Track)
                .Select(x => new
                    {
                        TrackId = x.Id,
                        TrackName = $"{x.Track.Name}: {x.Name} - {x.Length} м"
                    }).ToList();

            ViewData["Tracks"] = new SelectList(tracks, "TrackId", "TrackName");
            ViewData["Car"] = new SelectList(cars, "CarId", "Name");
            return View();
        }

        // POST: LapSessions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SessionViewModel session)
        {
            ViewBag.UserName = HttpContext.User.Identity.Name;
            ViewBag.FisrtName = HttpContext.User.Claims.Where(x => x.Type == "Name").Select(x => x.Value).First();
            ViewBag.Id = HttpContext.User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).First();

            if (ModelState.IsValid)
            {
                LapSession lapSession = new LapSession();
                lapSession.LapTime = session.Minutes * 60000 + session.Seconds*1000 + session.Miliseconds;
                lapSession.LapNote = session.LapNote;
                lapSession.AccountId = Convert.ToInt32(HttpContext.User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).First());
                lapSession.CarId = session.CarID;
                lapSession.TrackId = session.TrackConfId;
                lapSession.TrackDate = session.SessionDate;
                
                try
                {
                    _context.Sessions.Add(lapSession);
                    await _context.SaveChangesAsync();

                    ModelState.Clear();

                    ViewData["Message"] = $"Заезд создан.";
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Ошибка создания  заезда, попробуйте позже");
                    return View(session);
                }
                return View();
            }

            return View(session);
        }

        // GET: LapSessions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.UserName = HttpContext.User.Identity.Name;
            ViewBag.FisrtName = HttpContext.User.Claims.Where(x => x.Type == "Name").Select(x => x.Value).First();
            ViewBag.Id = HttpContext.User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).First();
            ViewBag.DefaultDate = DateTime.Today.Date.ToShortDateString();

            var userSession = await _context.Sessions
               .Include(l => l.Account)
               .Include(l => l.Car)
               .Include(l => l.Track)
               .Select(
                    session => new {
                        Id = session.Id,
                        PrintTime = (
                            $"{(session.LapTime / 60000).ToString().PadLeft(2, '0')}" +
                            $":{(session.LapTime % 60000 / 1000).ToString().PadLeft(2, '0')}" +
                            $":{(session.LapTime % 1000).ToString().PadLeft(3, '0')}"
                        ),
                        CarID = session.Car.Id,
                        CarName = $"{session.Car.Manufacturer} {session.Car.Model}",
                        UserId = session.Account.Id,
                        Username = $"{session.Account.Firstname} {session.Account.Lastname}",
                        TrackId = session.Track.Track.Id,
                        TrackConfigId = (int)session.TrackId,
                        TrackName = $"{session.Track.Track.Name}: {session.Track.Name} - {session.Track.Length} м",
                        SessionDate = session.TrackDate,
                        SessionNote = session.LapNote
                    }
                )
               .SingleOrDefaultAsync(us => us.Id == id);

            if (userSession == null) 
            {
                return NotFound();
            }
            if ((int)userSession.UserId != Convert.ToInt32(ViewBag.Id))
            {
                return RedirectToAction("MySessions");
            }

            ViewData["SessionID"] = userSession.Id;
            ViewData["Time"] = userSession.PrintTime;
            ViewData["Username"] = userSession.Username;
            ViewData["SessionDate"] = userSession.SessionDate;
            ViewData["Car"] = userSession.CarName;
            ViewData["Track"] = userSession.TrackName;
            ViewData["SessionNote"] = userSession.SessionNote;

            var cars = _context.Cars
                .Select(x => new
                {
                    CarId = x.Id,
                    Name = $"{x.Manufacturer} {x.Model} - {x.Description}"
                }).ToList();

            var tracks = _context.TrackConfigurations.Include(c => c.Track)
                .Select(x => new
                {
                    TrackId = x.Id,
                    TrackName = $"{x.Track.Name}: {x.Name} - {x.Length} м"
                }).ToList();

            ViewData["Tracks"] = new SelectList(tracks, "TrackId", "TrackName");
            ViewData["Cars"] = new SelectList(cars, "CarId", "Name");


            return View(new SessionViewModel());
        }

        // POST: LapSessions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SessionViewModel lapSession)
        {
            ViewBag.UserName = HttpContext.User.Identity.Name;
            ViewBag.FisrtName = HttpContext.User.Claims.Where(x => x.Type == "Name").Select(x => x.Value).First();
            ViewBag.Id = HttpContext.User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).First();

            if (id != lapSession.Id)
            {
                return NotFound();
            }

            if (lapSession.Miliseconds == 0 && lapSession.Seconds == 0 && lapSession.Minutes == 0)
            {
                ModelState.AddModelError("lap_time", "Время круга не может быть 0 милисекунд!");
            }

            if (ModelState.IsValid)
            {
                LapSession updateSession = new LapSession
                {
                    Id = id,
                    LapTime = lapSession.Minutes * 60000 + lapSession.Seconds * 1000 + lapSession.Miliseconds,
                    LapNote = lapSession.LapNote,
                    TrackDate = lapSession.SessionDate,
                    AccountId = Convert.ToInt32(HttpContext.User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).First()),
                    CarId = lapSession.CarID,
                    TrackId = lapSession.TrackConfId
                };

                try
                {
                    _context.Update(updateSession);
                    await _context.SaveChangesAsync();

                    ModelState.Clear();

                    ViewData["Message"] = $"Редактирование заезда завершено успешно.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LapSessionExists(updateSession.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("MySessions");
            }
            return RedirectToAction(nameof(Edit), new { id });
        }

        // POST: LapSessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lapSession = await _context.Sessions.FindAsync(id);
            if (lapSession != null)
            {
                _context.Sessions.Remove(lapSession);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("MySessions");
        }

        private bool LapSessionExists(int id)
        {
            return _context.Sessions.Any(e => e.Id == id);
        }
    }
}
