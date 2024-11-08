using Microsoft.AspNetCore.Mvc;

namespace DiamondShop.Repository.ViewModels.Request.Product;

public class QueryProductRequest
{
    [FromQuery(Name = "startPrice")]
    public decimal StartPrice { get; set; }

    [FromQuery(Name = "endPrice")]
    public decimal EndPrice { get; set; }

    [FromQuery(Name = "name")]
    public string Name { get; set; } = string.Empty;

    [FromQuery(Name = "categoryIds")]
    public List<Guid> CategoryIds { get; set; } = [];
        
    [FromQuery(Name = "material")]
    public string? Material { get; set; }
        
    [FromQuery(Name = "diamondIds")]
    public List<Guid> DiamondIds { get; set; } = [];
}