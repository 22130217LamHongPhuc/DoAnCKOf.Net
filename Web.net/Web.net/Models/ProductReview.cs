using System;
using System.Collections.Generic;

namespace API.Net.Models;

public partial class ProductReview
{



    public int Rating { get; set; }

    public string Comment { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string Fullname { get; set; } = null!;
    public string ProductName { get; set; } = null!;

    public int QuantitySell { get; set; } = 0!;

   
}
