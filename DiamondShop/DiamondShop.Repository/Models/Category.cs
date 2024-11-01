using System;
using System.Collections.Generic;

namespace DiamondShop.Repository.Models;

public partial class Category
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? LastUpdate { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
