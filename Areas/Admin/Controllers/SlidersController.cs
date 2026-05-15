using FurnishMvc.Data;
using FurnishMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FurnishMvc.Areas.Admin.Controllers;

[Area("Admin")]
public class SlidersController : Controller
{
    private readonly AppDbContext _context;

    public SlidersController(AppDbContext context)
    {
        _context = context;
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
        if (id is null)
        {
            return NotFound();
        }

        Slider? slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);

        if (slider is null)
        {
            return NotFound();
        }

        return View(slider);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("DiscountText,Title,Description,Price,ImageUrl,ButtonText,ButtonLink,Order,IsActive")] Slider slider)
    {
        if (!ModelState.IsValid)
        {
            return View(slider);
        }

        _context.Sliders.Add(slider);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Slider created successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        Slider? slider = await _context.Sliders.FindAsync(id);

        if (slider is null)
        {
            return NotFound();
        }

        return View(slider);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,DiscountText,Title,Description,Price,ImageUrl,ButtonText,ButtonLink,Order,IsActive")] Slider slider)
    {
        if (id != slider.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(slider);
        }

        try
        {
            _context.Sliders.Update(slider);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await SliderExists(slider.Id))
            {
                return NotFound();
            }

            throw;
        }

        TempData["SuccessMessage"] = "Slider updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        Slider? slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);

        if (slider is null)
        {
            return NotFound();
        }

        return View(slider);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        Slider? slider = await _context.Sliders.FindAsync(id);

        if (slider is null)
        {
            return NotFound();
        }

        _context.Sliders.Remove(slider);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Slider deleted successfully.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> SliderExists(int id)
    {
        return await _context.Sliders.AnyAsync(s => s.Id == id);
    }
}
