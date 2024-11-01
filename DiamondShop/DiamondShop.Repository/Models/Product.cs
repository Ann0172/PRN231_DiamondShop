using System;
using System.Collections.Generic;

namespace DiamondShop.Repository.Models;

public partial class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Material { get; set; }

    public bool? Gender { get; set; }

    public decimal Price { get; set; }

    public int Point { get; set; }

    public int Quantity { get; set; }

    public int WarrantyPeriod { get; set; }

    public DateTime? LastUpdate { get; set; }

    public string Status { get; set; } = null!;

    public Guid DiamondId { get; set; }

    public Guid CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Diamond Diamond { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Picture> Pictures { get; set; } = new List<Picture>();
}
