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
using Microsoft.AspNetCore.Identity;
using LMS.Core.Entities.ViewModels;

namespace LMS.Web.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> userManager;

        public UsersController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        // GET: Userss
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Users.Include(u => u.Course);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Userss/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            ///////////////////tillbaka till rätt flik////////////////
            var resStudent = await userManager.GetUsersInRoleAsync("Student");
            var indexViewModel = new IndexViewModel()
            {
                ListOfStudents = (List<User>)resStudent,
            };



            foreach (var student in resStudent)
            {
                if (student.Id == id)
                {
                    TempData["Origin"] = "op3";
                    break;
                }
                else
                {
                    TempData["Origin"] = "op2";
                }
            }
            ////////////////////////////////////////////////////

            var Users = await _context.Users
                .Include(u => u.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Users == null)
            {
                return NotFound();
            }

            return View(Users);
        }

        // GET: Users Teacher/Create
        public IActionResult CreateTeacher()
        {
           
            
            return View();
        }

        public IActionResult CreateMyStudent()
        {



            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Description");
            return View();
        }


        // POST: Userss/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTeacher(UserViewModel userViewModel)
        {
            try
            {
                var user = new User();
                user.FirstName = userViewModel.FirstName;
                user.LastName = userViewModel.LastName;
                user.Email = userViewModel.Email;
                user.UserName = userViewModel.Email;
                //user.PasswordHash = userViewModel.Password;
                string userPWD = "GetNewPassword!123";
                await userManager.CreateAsync(user, userPWD);
                await userManager.AddToRoleAsync(user, "Teacher");
                await _context.SaveChangesAsync();
                TempData["Message"] = "Teacher added";
                return Redirect("/Courses/IndexTeacher");
            }
            catch (Exception)
            {
                TempData["Message"] = "Teacher exist,Check Email";
                return Redirect("/Courses/IndexTeacher");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStudent(UserViewModel userViewModel)
        {
            try
            {
                var user = new User();
                user.FirstName = userViewModel.FirstName;
                user.LastName = userViewModel.LastName;
                user.Email = userViewModel.Email;
                user.UserName = userViewModel.Email;
                user.CourseId = userViewModel.CourseId;
                //user.PasswordHash = userViewModel.Password;
                string userPWD = "GetNewPassword!123";
                await userManager.CreateAsync(user, userPWD);
                await userManager.AddToRoleAsync(user, "Student");
                await _context.SaveChangesAsync();
                TempData["Message"] = "Student added";
                return Redirect("/Courses/IndexTeacher");
            }
            catch (Exception)
            {
                TempData["Message"] = "Student exist. Check Email";
                return Redirect("/Courses/IndexTeacher");
            }
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            

            if (id == null || _context.Users == null)
            {
                return NotFound();
            }



            var Users = await _context.Users.FindAsync(id);
            if (Users == null)
            {
                return NotFound();
            }

            ///////////////////STYR MOT RÄTT FLIK/////////////////////

            var resStudent = await userManager.GetUsersInRoleAsync("Student");
            var indexViewModel = new IndexViewModel()
            {
                ListOfStudents = (List<User>)resStudent,
            };

            foreach (var student in resStudent)
            {
                if (student.Id == id)
                {
                    TempData["Origin"] = "op3";
                    break;
                }
                else
                {
                    TempData["Origin"] = "op2";
                }
            }

            ///////////////////////////////////////////////////////////

            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Description", Users.CourseId);
            return View(Users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  User Users)
        {
            if (id != Users.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(Users);
                    //await _context.SaveChangesAsync();

                    var userToChange2 = await userManager.FindByEmailAsync(Users.Email);
                    userToChange2.FirstName = Users.FirstName;
                    userToChange2.LastName = Users.LastName;
                    userToChange2.Email = Users.Email;
                    userToChange2.PhoneNumber = Users.PhoneNumber;

                    await userManager.UpdateAsync(userToChange2);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(Users.Id))
                    {
                        return NotFound(); 
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["RouteOrigin"] = "StudentTab";

                return RedirectToAction("IndexTeacher", "Courses", new {origin = "op2"});
            }
            
            //ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Description", Users.CourseId);
            return View(Users);
        }

        // GET: Userss/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }


            ///////////////////tillbaka till rätt flik////////////////
            var resStudent = await userManager.GetUsersInRoleAsync("Student");
            var indexViewModel = new IndexViewModel()
            {
                ListOfStudents = (List<User>)resStudent,
            };



            foreach (var student in resStudent)
            {
                if (student.Id == id)
                {
                    TempData["Origin"] = "op3";
                    break;
                }
                else
                {
                    TempData["Origin"] = "op2";
                }
            }
            ////////////////////////////////////////////////////
            
            var Users = await _context.Users
                .Include(u => u.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Users == null)
            {
                return NotFound();
            }

            return View(Users);
        }

        // POST: Userss/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Users'  is null.");
            }
            var Users = await _context.Users.FindAsync(id);
            if (Users != null)
            {
                _context.Users.Remove(Users);
            }
            
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("IndexTeacher","Courses");

        }

        private bool UsersExists(int id)
        {
          return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }




    }




}
