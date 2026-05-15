namespace FurnishMvc.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public decimal OldPrice { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public int StockCount { get; set; }

        public bool IsFeatured { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;
    }
}
