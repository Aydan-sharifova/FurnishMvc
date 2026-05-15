namespace FurnishMvc.Models
{
    public class Slider
    {
        public int Id { get; set; }

        public string DiscountText { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public string ButtonText { get; set; } = string.Empty;

        public string ButtonLink { get; set; } = string.Empty;

        public int Order { get; set; }

        public bool IsActive { get; set; }
    }
}
