using FurnishMvc.Data;
using FurnishMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FurnishMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            HomeVM HomeVm = new HomeVM()
            {
                Sliders = await _context.Sliders
                    .Where(s => s.IsActive)
                    .OrderBy(s => s.Order)
                    .ToListAsync(),
                Products = await _context.Products
                    .Include(p => p.Category)
                    .Where(p => p.IsFeatured)
                    .OrderByDescending(p => p.Id)
                    .Take(3)
                    .ToListAsync()
            };

            return View(HomeVm);
        }
    }
}
