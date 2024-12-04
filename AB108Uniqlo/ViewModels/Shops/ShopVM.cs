using AB108Uniqlo.ViewModels.Brands;
using AB108Uniqlo.ViewModels.Products;

namespace AB108Uniqlo.ViewModels.Shops;

public class ShopVM
{
    public IEnumerable<BrandAndProductVM> Brands { get; set; }
    public IEnumerable<ProductListItemVM> Products { get; set; }
    public int ProductCount { get; set; }
}
