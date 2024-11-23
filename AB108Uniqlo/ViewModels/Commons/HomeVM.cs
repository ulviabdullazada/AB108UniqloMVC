using AB108Uniqlo.Models;
using AB108Uniqlo.ViewModels.Products;
using AB108Uniqlo.ViewModels.Sliders;

namespace AB108Uniqlo.ViewModels.Commons;

public class HomeVM
{
    public IEnumerable<SliderListItemVM> Sliders { get; set; }
    public IEnumerable<ProductListItemVM> Products { get; set; }
}
