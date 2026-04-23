using CakeShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CakeShop.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option) { }

        public DbSet<Cake> Cakes { get; set; }  
        public DbSet<Order> Orders { get; set; }    
    }
}
