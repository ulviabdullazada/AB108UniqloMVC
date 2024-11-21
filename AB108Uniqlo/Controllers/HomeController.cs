using AB108Uniqlo.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AB108Uniqlo.Controllers
{
    public class HomeController(UniqloDbContext _context) : Controller
    {
        
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sliders.ToListAsync());
        }
        public IActionResult About()
        {
            return View();
        }
    }
}
