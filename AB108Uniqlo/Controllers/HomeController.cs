using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AB108Uniqlo.Controllers
{
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
    }
}
