using System.ComponentModel.DataAnnotations;

namespace DiamondShop.Repository.ViewModels.Request.Category;

public class CreateCategoryRequest
{
    [Required]
    public required string Name { get; set; }
}