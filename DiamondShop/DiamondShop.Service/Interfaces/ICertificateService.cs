﻿using System.Security.Claims;
using DiamondShop.Repository.Pagination;
using DiamondShop.Repository.ViewModels.Response.Certificate;

namespace DiamondShop.Service.Interfaces;

public interface ICertificateService
{
    Task<Paginate<GetCertificatePagedResponse>> GetPagedCertificate(int page, int size);
    Task<GetCertificateProductPagedResponse> GetCertificateForAccount(ClaimsPrincipal claim);
}