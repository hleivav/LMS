﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LMS.Core.Entities;
using LMS.Data;
using LMS.Data.Data;
using Microsoft.AspNetCore.Authorization;

namespace LMS.Web.Controllers
{
    [Authorize]
    public class ActivityTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActivityTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ActivityTypes
        public async Task<IActionResult> Index()
        {
              return _context.ActivityType != null ? 
                          View(await _context.ActivityType.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.ActivityType'  is null.");
        }

        // GET: ActivityTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ActivityType == null)
            {
                return NotFound();
            }

            var activityType = await _context.ActivityType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityType == null)
            {
                return NotFound();
            }

            return View(activityType);
        }

        // GET: ActivityTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ActivityTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] ActivityType activityType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activityType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(activityType);
        }

        // GET: ActivityTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ActivityType == null)
            {
                return NotFound();
            }

            var activityType = await _context.ActivityType.FindAsync(id);
            if (activityType == null)
            {
                return NotFound();
            }
            return View(activityType);
        }

        // POST: ActivityTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] ActivityType activityType)
        {
            if (id != activityType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activityType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityTypeExists(activityType.Id))
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
            return View(activityType);
        }

        // GET: ActivityTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ActivityType == null)
            {
                return NotFound();
            }

            var activityType = await _context.ActivityType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityType == null)
            {
                return NotFound();
            }

            return View(activityType);
        }

        // POST: ActivityTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ActivityType == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ActivityType'  is null.");
            }
            var activityType = await _context.ActivityType.FindAsync(id);
            if (activityType != null)
            {
                _context.ActivityType.Remove(activityType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityTypeExists(int id)
        {
          return (_context.ActivityType?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
