using System;
using System.Collections.Generic;

namespace DiamondShop.Repository.Models;

public partial class Picture
{
    public Guid Id { get; set; }

    public string? UrlPath { get; set; }

    public Guid? DiamondId { get; set; }

    public Guid? ProductId { get; set; }

    public virtual Diamond? Diamond { get; set; }

    public virtual Product? Product { get; set; }
}
