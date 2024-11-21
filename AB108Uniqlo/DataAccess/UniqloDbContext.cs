using AB108Uniqlo.Models;
using Microsoft.EntityFrameworkCore;

namespace AB108Uniqlo.DataAccess
{
    public class UniqloDbContext : DbContext
    {
        public DbSet<Slider> Sliders { get; set; }
        public UniqloDbContext(DbContextOptions opt):base(opt){ }
    }
}
