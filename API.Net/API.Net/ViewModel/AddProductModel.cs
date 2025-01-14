﻿namespace API.Net.ViewModel
{
    public class AddProductModel
    {
        public string ProductId { get; set; } = null!;

        public string Name { get; set; } = null!;

        public int Price { get; set; }

        public int? DiscountDefault { get; set; }

        public string? Description { get; set; }

        public string? SubcategoryId { get; set; }

        public int QuanlityStock { get; set; }

        public int QuanlitySell { get; set; }

        public string Thumbnail { get; set; } = null!;

        public DateOnly CreateAt { get; set; }
    }
}
