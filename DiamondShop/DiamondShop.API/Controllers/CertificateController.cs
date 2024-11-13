using DiamondShop.Repository.Pagination;
using DiamondShop.Repository.ViewModels.Response.Certificate;
using DiamondShop.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondShop.API.Controllers
{
    [Route("api/certificates")]
    [ApiController]
    public class CertificateController : ControllerBase
    {
        private readonly ICertificateService _certificateService;

        public CertificateController(ICertificateService certificateService)
        {
            _certificateService = certificateService;
        }
        [HttpGet]
        public async Task<ActionResult<Paginate<GetCertificatePagedResponse>>> GetPagedCertificate([FromQuery] int page = 1,
            [FromQuery] int size = 10)
        {
            return await _certificateService.GetPagedCertificate(page, size);
        }
        [HttpGet("get-cerificate-by-current-account")]
        public async Task<ActionResult<GetCertificateProductPagedResponse>> GetCertificates()
        {
            return await _certificateService.GetCertificateForAccount(HttpContext.User);
        }
    }
}
