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
                Link = x.Link!,
                Subtitle = x.Subtitle,
                Title = x.Title
            }).ToListAsync();
            vm.Brands = await _context.Brands.OrderByDescending(x => x.Products!.Count)
                .Take(4).ToListAsync();
            vm.PopularProducts = await _context.Products
                .Where(x=> vm.Brands.Select(y=>y.Id).Contains(x.BrandId!.Value))
                .Take(10)
                .Select(x=> new ProductListItemVM
                {
                    CoverImage = x.CoverImage,
                    Discount = x.Discount,
                    Id = x.Id,
                    IsInStock = x.Quantity > 0,
                    Name = x.Name,
                    SellPrice = x.SellPrice,
                    BrandId = x.BrandId!.Value
                }).ToListAsync();
            
            return View(vm);
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        //public void SetSession(string key, string value)
        //{
        //    HttpContext.Session.SetString(key, value);
        //}
        //public IActionResult GetSession(string key)
        //{
        //    return Content(HttpContext.Session.GetString(key) ?? string.Empty);
        //}
        //public void SetCookie(string key, string value)
        //{
        //    var opt = new CookieOptions
        //    {
        //        Expires = new DateTime(2024, 11,30),
        //        //MaxAge = TimeSpan.FromMinutes(2)
        //    };
        //    HttpContext.Response.Cookies.Append(key, value);
        //}
        //public IActionResult GetCookie(string key)
        //{
        //    return Content(HttpContext.Request.Cookies[key]);
        //}
        //public IActionResult RemoveCookie(string key)
        //{
        //    HttpContext.Response.Cookies.Delete(key);
        //    return Ok();
        //}
    }
}
