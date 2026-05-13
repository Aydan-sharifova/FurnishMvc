namespace FurnishMvc.Models
{
    public class Slider
    {
        public int Id { get; set; }

        public string DiscountText { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public string ButtonText { get; set; }

        public string ButtonLink { get; set; }

        public int Order { get; set; }

        public bool IsActive { get; set; }
    }
}
