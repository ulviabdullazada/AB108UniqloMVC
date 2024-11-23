using AB108Uniqlo.Models;
using Microsoft.EntityFrameworkCore;

namespace AB108Uniqlo.DataAccess
{
    public class UniqloDbContext : DbContext
    {
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public UniqloDbContext(DbContextOptions opt):base(opt){ }
    }
}
