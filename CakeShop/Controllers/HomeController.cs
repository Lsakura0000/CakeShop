using CakeShop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CakeShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context; 
        }

        public async Task<IActionResult> Index()
        {
            if (!_context.Cakes.Any())
            {
                _context.Cakes.AddRange
                    (
                    new Models.Cake { Name = "草莓千層", Price = 120, Description = "新鮮草莓" },
                    new Models.Cake { Name = "重奶酪蛋糕", Price = 100, Description = "經典美式重奶酪" },
                    new Models.Cake { Name = "黑森林蛋糕", Price = 90, Description = "苦甜巧克力" }
                    );
                await _context.SaveChangesAsync();
            }

            var cakes = await _context.Cakes.ToListAsync();
            return View(cakes); 
        }
    }
}
