using CakeShop.Data;
using CakeShop.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CakeShop.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManger;
        private readonly IEmailSender _emailSender;

        public OrdersController(ApplicationDbContext context, UserManager<IdentityUser> userManger, IEmailSender emailSender)
        {
            _context = context;
            _userManger = userManger;
            _emailSender = emailSender;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int cakeId, int quantity)
        {
            var cake = await _context.Cakes.FindAsync(cakeId);
            if (cake == null) return NotFound();

            var user = await _userManger.GetUserAsync(User);

            var order = new Order
            {
                UserId = user.Id,
                CakeId = cakeId,
                Quantity = quantity,
                TotalPrice = cake.Price * quantity
            };

            _context.Orders.Add(order); 
            await _context.SaveChangesAsync();

            string subject = "訂單已確認";
            string message = $"親愛的顧客您好，<br>您已成功訂購 {quantity} 份 {cake.Name}，總價為 {order.TotalPrice} 元。<br>感謝您的購買！";
            await _emailSender.SendEmailAsync(user.Email,subject,message);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManger.GetUserAsync(User);

            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id && o.UserId == user.Id);

            if (order == null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
