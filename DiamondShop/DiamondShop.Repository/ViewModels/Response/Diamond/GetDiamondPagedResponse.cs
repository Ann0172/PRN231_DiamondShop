using DiamondShop.Repository.ViewModels.Response.Certificate;
using DiamondShop.Repository.ViewModels.Response.Picture;

namespace DiamondShop.Repository.ViewModels.Response.Diamond;

public class GetDiamondPagedResponse
{
    public Guid Id { get; set; }
        
    public string? Shape { get; set; }

    public string? Color { get; set; }

    public string? Origin { get; set; }

    public string? CaratWeight { get; set; }

    public string? Clarity { get; set; }

    public string? Cut { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public int WarrantyPeriod { get; set; }

    public DateTime? LastUpdate { get; set; }

    public string? Status { get; set; }
    
    public GetCertificateResponse? Certificate { get; set; }
    
    public ICollection<GetPictureResponse> Pictures { get; set; } = new List<GetPictureResponse>();
}