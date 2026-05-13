namespace FurnishMvc.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public string? Icon { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
