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
using Microsoft.AspNetCore.Identity;
using LMS.Core.Entities.ViewModels;

namespace LMS.Web.Controllers
{
    public class ModulesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private static UserManager<User> userManager = default;

        public ModulesController(ApplicationDbContext context)
        {
            _context = context;
           
        }

        // GET: Modules
        public async Task<IActionResult> Index()
        {
              return _context.Module != null ? 
                          View(await _context.Module.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Module'  is null.");
        }

        // GET: Modules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Module == null)
            {
                return NotFound();
            }

            var @module = await _context.Module
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@module == null)
            {
                return NotFound();
            }

            return View(@module);
        }

        // GET: Modules/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Modules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(/*[Bind("Id,ModuleName,ModuleDescription,ModuleStarDate,ModuleEndDate")]*/ CoursesViewModel @module)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var myModule=new Module();
        //        myModule.CourseId = @module.Id;
        //        myModule.StartDate = @module.StartDate;
        //        myModule.EndDate = @module.EndDate;
        //        myModule.Description = @module.Description;
        //        myModule.Name = @module.Name;
        //        _context.Module.Add(myModule);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    //return View(@module);
        //    return RedirectToAction(nameof(Details), "Courses", new { id = module.Id});
        //}

        // GET: Modules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var module = await _context.Module.FindAsync(id);
            ModulesViewModel moduleViewModel = new ModulesViewModel()
            {
                Name = module.Name,
                Description = module.Description,
                StartDate = module.StartDate,
                EndDate = module.EndDate,
                CourseId = forwardId,
                ForwardCourseId = forwardId
            };
            if (module == null)
            {
                return NotFound();
            }
            return View(@module);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ModulesViewModel modelView)
        {
           try
            {
                var module = await _context.Module.FindAsync(id);
                module.Name = modelView.Name;
                module.Description = modelView.Description;
                module.StartDate = modelView.StartDate;
                module.EndDate = modelView.EndDate;
                _context.Update(module);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["Message"] = "Module not updated";
                string strr = "/Courses/Details/" + modelView.ForwardCourseId;
                return Redirect(strr);
            }                      
            TempData["Message"] = "Module \"" + modelView.Name + "\" is updated";
            string str = "/Courses/Details/" + modelView.ForwardCourseId;
            return Redirect(str);           
        }

        // GET: Modules/Delete/5
        public async Task<IActionResult> Delete(int? id, int forwardId)
        {
            if (id == null || _context.Module == null)
            {
                return NotFound();
            }
            var module = await _context.Module.FirstOrDefaultAsync(m => m.Id == id);
            var modulesViewModel = new ModulesViewModel()
            {
                Name = module.Name,
                Description = module.Description,
                StartDate = module.StartDate,
                EndDate = module.EndDate,
                CourseId = module.CourseId,
                ForwardCourseId = forwardId
            };
            if (module == null)
            {
                return NotFound();
            }

            return View(modulesViewModel);
        }

        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, ModulesViewModel modelView)
        {
            if (_context.Module == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Module'  is null.");
            }
            var @module = await _context.Module.FindAsync(id);
            if (@module != null)
            {
                _context.Module.Remove(@module);
            }
            
            await _context.SaveChangesAsync();
            TempData["Message"] = "Module is Deleted";
            string str = "/Courses/Details/" + modelView.ForwardCourseId;
            return Redirect(str);
           // return RedirectToAction(nameof(Index));
        }

        private bool ModuleExists(int id)
        {
          return (_context.Module?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
