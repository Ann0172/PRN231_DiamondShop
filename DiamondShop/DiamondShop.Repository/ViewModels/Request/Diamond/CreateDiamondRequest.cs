using System.ComponentModel.DataAnnotations;
using DiamondShop.Repository.Enums;
using Microsoft.AspNetCore.Http;

namespace DiamondShop.Repository.ViewModels.Request.Diamond;

public class CreateDiamondRequest
{
    [Required]
    public string? ReportNumber { get; set; }
    [Required]
    [EnumDataType(typeof(DiamondShape))]
    public required string Shape { get; set; }
    [Required]
    [EnumDataType(typeof(DiamondColor))]
    public required string Color { get; set; }

    [Required]
    [EnumDataType(typeof(DiamondOrigin))]
    public required string Origin { get; set; }

    [Required]
    public required string CaratWeight { get; set; }

    [Required]
    [EnumDataType(typeof(DiamondClarity))]
    public required string Clarity { get; set; }

    [Required]
    [EnumDataType(typeof(DiamondCut))]
    public required string Cut { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public int Quantity { get; set; }

    public int WarrantyPeriod { get; set; }
    
    public List<IFormFile> DiamondImages { get; set; } = [];
}