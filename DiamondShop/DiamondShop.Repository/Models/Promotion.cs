using System;
using System.Collections.Generic;

namespace DiamondShop.Repository.Models;

public partial class Promotion
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime ExpiredDate { get; set; }

    public int DiscountPercent { get; set; }

    public string Status { get; set; } = null!;
}
