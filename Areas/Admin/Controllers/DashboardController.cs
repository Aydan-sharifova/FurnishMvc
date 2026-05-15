using FurnishMvc.Data;
using FurnishMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FurnishMvc.Areas.Admin.Controllers;

[Area("Admin")]
public class DashboardController : Controller
{
    private readonly AppDbContext _context;

    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.CategoryCount = await _context.Categories.CountAsync();
        ViewBag.ProductCount = await _context.Products.CountAsync();
        ViewBag.SliderCount = await _context.Sliders.CountAsync();
        ViewBag.ActiveSliderCount = await _context.Sliders.CountAsync(s => s.IsActive);

        List<Product> recentProducts = await _context.Products
            .Include(p => p.Category)
            .OrderByDescending(p => p.Id)
            .Take(5)
            .ToListAsync();

        return View(recentProducts);
    }
}
