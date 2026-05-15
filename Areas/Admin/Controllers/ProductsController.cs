using FurnishMvc.Data;
using FurnishMvc.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FurnishMvc.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductsController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public ProductsController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        List<Product> products = await _context.Products
            .Include(p => p.Category)
            .OrderBy(p => p.Name)
            .ToListAsync();

        return View(products);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();

        Product? product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product is null) return NotFound();

        return View(product);
    }

    public async Task<IActionResult> Create()
    {
        await PopulateCategoriesAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Name,Description,Price,OldPrice,Photo,StockCount,IsFeatured,CategoryId")] Product product)
    {
        ModelState.Remove(nameof(Product.Category));
        ModelState.Remove(nameof(Product.ImageUrl));

        if (!ModelState.IsValid)
        {
            await PopulateCategoriesAsync(product.CategoryId);
            return View(product);
        }

        if (product.Photo != null)
        {
            product.ImageUrl = await SavePhotoAsync(product.Photo);
        }

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Product created successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();

        Product? product = await _context.Products.FindAsync(id);

        if (product is null) return NotFound();

        await PopulateCategoriesAsync(product.CategoryId);
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,Name,Description,Price,OldPrice,Photo,StockCount,IsFeatured,CategoryId")] Product product)
    {
        if (id != product.Id) return NotFound();

        ModelState.Remove(nameof(Product.Category));
        ModelState.Remove(nameof(Product.ImageUrl));

        if (!ModelState.IsValid)
        {
            await PopulateCategoriesAsync(product.CategoryId);
            return View(product);
        }

        Product? dbProduct = await _context.Products.FindAsync(id);

        if (dbProduct is null) return NotFound();

        dbProduct.Name = product.Name;
        dbProduct.Description = product.Description;
        dbProduct.Price = product.Price;
        dbProduct.OldPrice = product.OldPrice;
        dbProduct.StockCount = product.StockCount;
        dbProduct.IsFeatured = product.IsFeatured;
        dbProduct.CategoryId = product.CategoryId;

        if (product.Photo != null)
        {
            DeletePhoto(dbProduct.ImageUrl);
            dbProduct.ImageUrl = await SavePhotoAsync(product.Photo);
        }

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Product updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();

        Product? product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product is null) return NotFound();

        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        Product? product = await _context.Products.FindAsync(id);

        if (product is null) return NotFound();

        DeletePhoto(product.ImageUrl);

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Product deleted successfully.";
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateCategoriesAsync(object? selectedCategory = null)
    {
        List<Category> categories = await _context.Categories
            .OrderBy(c => c.Name)
            .ToListAsync();

        ViewBag.CategoryId = new SelectList(
            categories,
            nameof(Category.Id),
            nameof(Category.Name),
            selectedCategory);
    }

    private async Task<string> SavePhotoAsync(IFormFile photo)
    {
        string fileName = Guid.NewGuid() + Path.GetExtension(photo.FileName);

        string folderPath = Path.Combine(_env.WebRootPath, "uploads", "products");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string filePath = Path.Combine(folderPath, fileName);

        using FileStream stream = new FileStream(filePath, FileMode.Create);
        await photo.CopyToAsync(stream);

        return "/uploads/products/" + fileName;
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