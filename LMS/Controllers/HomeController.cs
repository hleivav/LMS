using LMS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace LMS.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IHostingEnvironment _hostingEnv;
        public HomeController(ILogger<HomeController> logger, IHostingEnvironment hostingEnv)
        {
            _hostingEnv = hostingEnv;
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("Student"))
                return View(); 
                //return RedirectToPage("/Home/IndexStudent.cshtml");
            return RedirectToAction(nameof(IndexTeacher));
        }


        public IActionResult IndexTeacher()
        {

            return View();

        }

        public IActionResult Index2()
        {

            return View();

        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

       


    }

}