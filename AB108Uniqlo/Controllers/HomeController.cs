using AB108Uniqlo.DataAccess;
using AB108Uniqlo.Models;
using AB108Uniqlo.ViewModels.Commons;
using AB108Uniqlo.ViewModels.Products;
using AB108Uniqlo.ViewModels.Sliders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AB108Uniqlo.Controllers
{
    public class HomeController(UniqloDbContext _context) : Controller
    {
        
        public async Task<IActionResult> Index()
        {
            HomeVM vm = new();
            vm.Sliders = await _context.Sliders.Select(x => new SliderListItemVM
            {
                ImageUrl = x.ImageUrl,
                Link = x.Link,
                Subtitle = x.Subtitle,
                Title = x.Title
            }).ToListAsync();
            vm.Products = await _context.Products.Select(x=> new ProductListItemVM
            {
                CoverImage = x.CoverImage,
                Discount = x.Discount,
                Id = x.Id,
                IsInStock = x.Quantity > 0,
                Name = x.Name,
                SellPrice = x.SellPrice
            }).ToListAsync();
            return View(vm);
        }
        public IActionResult About()
        {
            return View();
        }
    }
}
