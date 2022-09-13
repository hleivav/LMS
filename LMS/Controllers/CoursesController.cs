using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LMS.Core.Entities;
using LMS.Data.Data;
using Microsoft.AspNetCore.Authorization;
using LMS.Core.Entities.ViewModels;

namespace LMS.Web.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private Task<string?> indexViewModel;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
              return _context.Course != null ? 
                          View(await _context.Course.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Course'  is null.");
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StarDate,EndDate")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StarDate,EndDate")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
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
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Course == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Course'  is null.");
            }
            var course = await _context.Course.FindAsync(id);
            if (course != null)
            {
                _context.Course.Remove(course);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
          return (_context.Course?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        public async Task<IActionResult> IndexStudent()
        {
            
            var resCourse = await _context.Course.Include(g=>g.Users).ToListAsync();
            var resUser = await _context.Users.ToListAsync();
            var resModule = await _context.Module.ToListAsync();
            var resActivity = await _context.Activity.ToListAsync();
            var resActivityType = await _context.ActivityType.ToListAsync();

            var indexViewModel = new IndexViewModel()
            {
                ListOfCourses = resCourse,
                ListOfUsers = resUser,
                ListOfModules = resModule,
                ListOfActivity = resActivity,
                ListOfActivityType = resActivityType

            };
            
            if (User.IsInRole("Student"))
            {
                
                return View(indexViewModel);

            }


            //return RedirectToPage("/Home/IndexStudent.cshtml");
            return  View(nameof(IndexTeacher), indexViewModel);
        }


        public async Task<IActionResult> IndexTeacher()
        {

            var resCourse = await _context.Course.Include(g => g.Users).ToListAsync();
            var resUser = await _context.Users.ToListAsync();
            var resModule = await _context.Module.ToListAsync();
            var resActivity = await _context.Activity.ToListAsync();
            var resActivityType = await _context.ActivityType.ToListAsync();

            var indexViewModel = new IndexViewModel()
            {
                ListOfCourses = resCourse,
                ListOfUsers = resUser,
                ListOfModules = resModule,
                ListOfActivity = resActivity,
                ListOfActivityType = resActivityType

            };

            
            return _context.Course != null ?
                          View(indexViewModel) :
                          Problem("Entity set 'ApplicationDbContext.Course'  is null.");

        }


    }
}
