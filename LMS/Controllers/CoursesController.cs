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
using Microsoft.AspNetCore.Identity;

namespace LMS.Web.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private Task<string?> indexViewModel;
        private readonly UserManager<User> userManager;
        public CoursesController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            this.userManager = userManager;
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

            var course = await _context.Course.FirstOrDefaultAsync(m => m.Id == id);
            var resActivity = await _context.Activity.ToListAsync();
            var resActivityType = await _context.ActivityType.ToListAsync();

            var module = _context.Module
             .Where(v => v.CourseId == id)
             .ToList();

            CoursesViewModel coursesViewModel = new CoursesViewModel()
            {
                Id = (int)id,
                listOfModules = module,
                ListOfActivity = resActivity,
                ListOfActivityType = resActivityType,
                Name = course.Name,
                Description = course.Description,
                StartDate = course.StartDate,
                EndDate = course.EndDate

            };
            if (course == null)
            {
                return NotFound();
            }

            return View(coursesViewModel);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateModule(CoursesViewModel viewModel)
        {
           CoursesViewModel coursesViewModel=null;
            if (ModelState.IsValid)
            {
                var module2 = new Module
                {
                    CourseId = viewModel.Id,
                    Name = viewModel.ModuleName,
                    Description = viewModel.ModuleDescription,
                    StartDate = viewModel.ModuleStartDate,
                    EndDate = viewModel.ModuleEndDate                  
                };
                _context.Module.Add(module2);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Module \""+ viewModel.ModuleName+"\" Added ";
                return RedirectToAction(nameof(Details), viewModel);// new { id = viewModel.Id });
            }

            if (!ModelState.IsValid)
            {
                var course = await _context.Course.FirstOrDefaultAsync(m => m.Id == viewModel.Id);
                var resActivity = await _context.Activity.ToListAsync();
                var resActivityType = await _context.ActivityType.ToListAsync();

                var module = _context.Module
                 .Where(v => v.CourseId == viewModel.Id)
                 .ToList();

                 coursesViewModel = new CoursesViewModel()
                {
                    Id = (int)viewModel.Id,
                    listOfModules = module,
                    ListOfActivity = resActivity,
                    ListOfActivityType = resActivityType,
                    Name = course.Name,
                    Description = course.Description,
                    StartDate = course.StartDate,
                    EndDate = course.EndDate                   
                };
             TempData["Message"] = "Not Added ";
             }

            return View("Details",coursesViewModel);
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
                return RedirectToAction(nameof(IndexTeacher));
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
            var course = await _context.Course
                .Include(c => c.Users)
                .Include(c => c.Modules)
                .ThenInclude(m => m.Activities)
                .FirstOrDefaultAsync(m => m.Id == id);
                //var module = _context.Module
                //                 .Where(v => v.CourseId == id)
                //                 .ToList();

                //int i = 0;
                //while(module.Count > 0)
                //{
                //    var activities = _context.Activity
                //        .Where(m => m.ModuleId == module[i].Id)
                //        .ToList();
                    
                //    if (module[i].Activities.Count > 0)
                //    {
                //        for (int j = 0; j < activities.Count - 1; j++)
                //        {
                //            _context.Activity.Remove(activities[j]); //bort med alla aktiviteter
                //            int xxx = j;
                //            await _context.SaveChangesAsync();
                //        }
                //    }
                //    _context.Module.Remove(module[i]); //bort med modulen när aktiviteterna är borttagna eller när de inte fanns alls.
                //    i++;
                //    await _context.SaveChangesAsync();
                //}
                
            _context.Course.Remove(course); //bort med kursen när inga moduler finns.
            _context.RemoveRange(course.Users);
 
            await _context.SaveChangesAsync();////den här
            return RedirectToAction(nameof(IndexTeacher));
        }

        private bool CourseExists(int id)
        {
          return (_context.Course?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        public async Task<IActionResult> IndexStudent()
        {

            if (TempData["Origin"] is null) //test
            {
                TempData["Origin"] = "op1";
            }
            //var test = TempData["Origin"]; //test

            var resCourse = await _context.Course.Include(g=>g.Users).ToListAsync();
            var resTeacher = await userManager.GetUsersInRoleAsync("Teacher");
            var resStudent = await userManager.GetUsersInRoleAsync("Student");
            var resModule = await _context.Module.ToListAsync();
            var resActivity = await _context.Activity.ToListAsync();
            var resActivityType = await _context.ActivityType.ToListAsync();
            var resUsers= await _context.Users.ToListAsync();

            var indexViewModel = new IndexViewModel()
            {
                ListOfCourses = resCourse,
                ListOfTeachers = (List<User>)resTeacher,
                ListOfStudents= (List<User>)resStudent,
                ListOfModules = resModule,
                ListOfActivity = resActivity,
                ListOfActivityType = resActivityType,
                ListOfUsers =resUsers
            };
            
            if (User.IsInRole("Student"))
            {
                
                return View(indexViewModel);

            }


            //return RedirectToPage("/Home/IndexStudent.cshtml");
            return  View(nameof(IndexTeacher), indexViewModel);
        }


        public async Task<IActionResult> IndexTeacher(string origin)
        {
            if (TempData["Origin"] is null)
            {
                TempData["Origin"] = "op1";
            }
            //var test = TempData["Origin"];
            var resCourse = await _context.Course.Include(g => g.Users).ToListAsync();
            var resTeacher = await userManager.GetUsersInRoleAsync("Teacher");
            var resStudent = await userManager.GetUsersInRoleAsync("Student");
            var resModule = await _context.Module.ToListAsync();
            var resActivity = await _context.Activity.ToListAsync();
            var resActivityType = await _context.ActivityType.ToListAsync();
            var resUsers = await _context.Users.ToListAsync();

            var indexViewModel = new IndexViewModel()
            {
                ListOfCourses = resCourse,
                ListOfTeachers = (List<User>)resTeacher,
                ListOfStudents = (List<User>)resStudent,
                ListOfModules = resModule,
                ListOfActivity = resActivity,
                ListOfActivityType = resActivityType,
                ListOfUsers = resUsers


            };

            
            return _context.Course != null ?
                          View(indexViewModel) :
                          Problem("Entity set 'ApplicationDbContext.Course'  is null.");

        }


    }
}
