﻿using DiamondShop.Repository.ViewModels.Response.Category;

namespace DiamondShop.Repository.ViewModels.Response.Product;

public class GetProductPagedResponse
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Material { get; set; }

    public bool? Gender { get; set; }

    public decimal Price { get; set; }

    public int Point { get; set; }

    public int Quantity { get; set; }

    public int WarrantyPeriod { get; set; }

    public DateTime? LastUpdate { get; set; }

    public string? Status { get; set; }

    public GetCategoryResponse Category { get; set; } = null!;
}