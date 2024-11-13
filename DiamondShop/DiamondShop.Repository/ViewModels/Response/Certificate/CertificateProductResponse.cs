using DiamondShop.Repository.ViewModels.Response.Product;

namespace DiamondShop.Repository.ViewModels.Response.Certificate;

public class CertificateProductResponse
{
    public GetCertificatePagedResponse? Certificate { get; set; }
    public GetProductResponse? Product { get; set; }
}