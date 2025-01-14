using API.Net.Models;

namespace API.Net.ViewModel
{
    public class AddSubImageViiewModel
    {
        public int SubImagesId { get; set; }

        public string Image { get; set; } = null!;

        public string ProductId { get; set; } = null!;


    }
}
