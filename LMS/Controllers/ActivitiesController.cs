using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LMS.Core.Entities;
using LMS.Data;
using LMS.Data.Data;
using LMS.Core.Entities.ViewModels;

namespace LMS.Web.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActivitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Activities
        public async Task<IActionResult> Index()
        {
              return _context.Activity != null ? 
                          View(await _context.Activity.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Activity'  is null.");
        }

        // GET: Activities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Activity == null)
            {
                return NotFound();
            }

            var activity = await _context.Activity
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // GET: Activities/Create
        public async Task<IActionResult> Create(int id, int forwardId, string acName)
        {
            var resActivityType = await _context.ActivityType.ToListAsync();
            var activityViewModel = new ActivityViewModel
            {
                ModuleId = id ,
                ForwardCourseId =forwardId,
                ActivityName = acName
            };
            ViewData["ActivityTypeId"] = new SelectList(_context.Set<ActivityType>(), "Id", "Name");
            return View("Create",activityViewModel);
        }

        // POST: Activities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActivityViewModel activity)
        {
            if (ModelState.IsValid)
            {
                var ac = new Activity
                {
                    ModuleId = activity.ModuleId,
                    Name = activity.Name,
                    Description = activity.Description,
                    StartDate = activity.StartDate,
                    EndDate = activity.EndDate,
                    ActivityTypeId=activity.ActivityTypeId
                };
                _context.Activity.Add(ac);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Activity \""+activity.Name+"\" added to module \"" + activity.ModuleId+"\"";
                string strr = "/Courses/Details/" + activity.ForwardCourseId.ToString();
                return Redirect(strr);
            }
            TempData["Message"] = "Activity Not added to module";
            string str = "/Courses/Details/" + activity.ForwardCourseId.ToString();
            return Redirect(str);
           
        }

        // GET: Activities/Edit/5
        public async Task<IActionResult> Edit(int id, int forwardId)
        {
           var activity = await _context.Activity.FindAsync(id);
            ActivityViewModel activityViewModel = new ActivityViewModel()
            {
                Id = id,
                Name = activity?.Name,
                Description = activity?.Description,
                StartDate = activity.StartDate,
                EndDate = activity.EndDate,
                ForwardCourseId = forwardId,
                ActivityTypeId = activity.ActivityTypeId
            };
            if (activity == null)
            {
                return NotFound();
            }
            
            ViewData["ActivityTypeId"] = new SelectList(_context.Set<ActivityType>(), "Id", "Name");
            return View(activityViewModel);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ActivityViewModel activity)
        {
            try
                {
                    var ac= await _context.Activity.FindAsync(id);
               
                    ac.Name = activity.Name;
                    ac.StartDate = activity.StartDate;
                    ac.EndDate = activity.EndDate;
                    ac.Description = activity.Description;
                    ac.ActivityTypeId=activity.ActivityTypeId;

                _context.Update(ac);
                await _context.SaveChangesAsync();
                 }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(activity.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            TempData["Message"] = "Activity \"" + activity.Name + "\" is updated";
            string str = "/Courses/Details/" + activity.ForwardCourseId.ToString();
            return Redirect(str);
           
        }

        

        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(int? id , int forwardId)
        {
            //if (id == null || _context.Activity == null)
            //{
            //    return NotFound();
            //}

            var activity = await _context.Activity
                .FirstOrDefaultAsync(m => m.Id == id);           
            if (activity == null)
            {
                return NotFound();
            }
            _context.Activity.Remove(activity);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Activity \"" + activity.Name + "\" is deleted";
            string str = "/Courses/Details/" + forwardId.ToString();
            return Redirect(str);
            //return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Activity == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Activity'  is null.");
            }
            var activity = await _context.Activity.FindAsync(id);
            if (activity != null)
            {
                _context.Activity.Remove(activity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityExists(int id)
        {
          return (_context.Activity?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
