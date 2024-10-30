using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScopeLap.DataBaseEngine;
using ScopeLap.Models.DataBaseEngine;

namespace ScopeLap.Controllers
{
    [Authorize]
    public class CarsController : Controller
    {
        private readonly ScopeLapDbContext _context;

        public CarsController(ScopeLapDbContext context)
        {
            _context = context;
        }

        // GET: Cars
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Cars.ToListAsync());
        //}

        //// GET: Cars/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var car = await _context.Cars
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (car == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(car);
        //}

        // GET: Cars/Create
        public IActionResult Create()
        {
            return PartialView("_addCar");
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Manufacturer,Model,Description,Modified,ModDescription")] Car car)
        {
            if (ModelState.IsValid)
            {
                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "LapSessions");
            }
            else
            {
                TempData["ErrorMessage"] = string.Join(",",
                    ModelState.Values.Where(E => E.Errors.Count > 0)
                    .SelectMany(E => E.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToArray());
            }
            return RedirectToAction("Create", "LapSessions");
        }

        //// GET: Cars/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var car = await _context.Cars.FindAsync(id);
        //    if (car == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(car);
        //}

        //// POST: Cars/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Manufacturer,Model,Description,Modified,ModDescription")] Car car)
        //{
        //    if (id != car.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(car);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!CarExists(car.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(car);
        //}

        //// GET: Cars/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var car = await _context.Cars
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (car == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(car);
        //}

        //// POST: Cars/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var car = await _context.Cars.FindAsync(id);
        //    if (car != null)
        //    {
        //        _context.Cars.Remove(car);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool CarExists(int id)
        //{
        //    return _context.Cars.Any(e => e.Id == id);
        //}
    }
}
