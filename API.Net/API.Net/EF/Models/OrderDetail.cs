﻿using System;
using System.Collections.Generic;

namespace API.Net.Models;

public partial class OrderDetail
{
    public int OrderId { get; set; }

    public string ProductId { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
