using FurnishMvc.Models;

namespace FurnishMvc.ViewModels
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; } = new();
        public List<Product> Products { get; set; } = new();
    }
}
