using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VP_Lab_13_Do_It_Yourself.Models;

namespace VP_Lab_13_Do_It_Yourself.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Registration()
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
