﻿using AB108Uniqlo.DataAccess;
using AB108Uniqlo.Helpers;
using AB108Uniqlo.ViewModels.Baskets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AB108Uniqlo.ViewComponents
{
    public class LayoutHeaderViewComponent(UniqloDbContext _context) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var basket = BasketHelper.GetBasket(HttpContext.Request);

            var basketItems = await _context.Products
                .Where(x => basket.Select(y => y.Id).Contains(x.Id))
                .Select(x=> new BasketItemVM
                {
                    Id = x.Id,
                    Name = x.Name,
                    ImageUrl = x.CoverImage,
                    SellPrice = x.SellPrice,
                    Discount = x.Discount
                })
                .ToListAsync();

            foreach (var item in basketItems)
                item.Count = basket.First(x=> x.Id == item.Id).Count;

            return View(basketItems);
        }
    }
}
