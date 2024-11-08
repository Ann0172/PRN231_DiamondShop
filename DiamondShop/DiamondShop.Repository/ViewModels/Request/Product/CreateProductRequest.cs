using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DiamondShop.Repository.ViewModels.Request.Product;

public class CreateProductRequest
{
    [MaxLength(255)]
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string Type { get; set; }
    [MaxLength(255)]
    [Required]
    public required string Material { get; set; }
    [Required]
    public bool Gender { get; set; }
    [Required]
    public decimal Price { get; set; }
    [Required]
    public int Point { get; set; }
    [Required]
    public int Quantity { get; set; }
    public int WarrantyPeriod { get; set; }
    [Required]
    public Guid DiamondId { get; set; }
    [Required]
    public Guid CategoryId { get; set; }
    public List<IFormFile> ProductPicture { get; set; } = [];
}