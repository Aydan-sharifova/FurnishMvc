using FurnishMvc.Data;
using FurnishMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FurnishMvc.Areas.Admin.Controllers;

[Area("Admin")]
public class SlidersController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public SlidersController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        List<Slider> sliders = await _context.Sliders
            .OrderBy(s => s.Order)
            .ThenBy(s => s.Title)
            .ToListAsync();

        return View(sliders);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();

        Slider? slider = await _context.Sliders
            .FirstOrDefaultAsync(s => s.Id == id);

        if (slider is null) return NotFound();

        return View(slider);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("DiscountText,Title,Description,Price,Photo,ButtonText,ButtonLink,Order,IsActive")] Slider slider)
    {
        ModelState.Remove(nameof(Slider.ImageUrl));

        if (!ModelState.IsValid)
        {
            return View(slider);
        }

        if (slider.Photo != null)
        {
            slider.ImageUrl = await SavePhotoAsync(slider.Photo);
        }

        _context.Sliders.Add(slider);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Slider created successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();

        Slider? slider = await _context.Sliders.FindAsync(id);

        if (slider is null) return NotFound();

        return View(slider);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,DiscountText,Title,Description,Price,Photo,ButtonText,ButtonLink,Order,IsActive")] Slider slider)
    {
        if (id != slider.Id) return NotFound();

        ModelState.Remove(nameof(Slider.ImageUrl));

        if (!ModelState.IsValid)
        {
            return View(slider);
        }

        Slider? dbSlider = await _context.Sliders.FindAsync(id);

        if (dbSlider is null) return NotFound();

        dbSlider.DiscountText = slider.DiscountText;
        dbSlider.Title = slider.Title;
        dbSlider.Description = slider.Description;
        dbSlider.Price = slider.Price;
        dbSlider.ButtonText = slider.ButtonText;
        dbSlider.ButtonLink = slider.ButtonLink;
        dbSlider.Order = slider.Order;
        dbSlider.IsActive = slider.IsActive;

        if (slider.Photo != null)
        {
            DeletePhoto(dbSlider.ImageUrl);
            dbSlider.ImageUrl = await SavePhotoAsync(slider.Photo);
        }

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Slider updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();

        Slider? slider = await _context.Sliders
            .FirstOrDefaultAsync(s => s.Id == id);

        if (slider is null) return NotFound();

        return View(slider);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        Slider? slider = await _context.Sliders.FindAsync(id);

        if (slider is null) return NotFound();

        DeletePhoto(slider.ImageUrl);

        _context.Sliders.Remove(slider);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Slider deleted successfully.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<string> SavePhotoAsync(IFormFile photo)
    {
        string fileName = Guid.NewGuid() + Path.GetExtension(photo.FileName);

        string folderPath = Path.Combine(_env.WebRootPath, "uploads", "sliders");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string filePath = Path.Combine(folderPath, fileName);

        using FileStream stream = new FileStream(filePath, FileMode.Create);
        await photo.CopyToAsync(stream);

        return "/uploads/sliders/" + fileName;
    }

    private void DeletePhoto(string? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl)) return;

        string filePath = Path.Combine(
            _env.WebRootPath,
            imageUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));

        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }
    }
}