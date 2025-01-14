namespace API.net.ViewModel;
[Serializable]

public class UpdateSpecModel
    {
        public string SpecificationId { get; set; } = null!;

        public string? Dimensions { get; set; }

        public string? Material { get; set; }

        public string? Original { get; set; }

        public string? Standard { get; set; }
    }
