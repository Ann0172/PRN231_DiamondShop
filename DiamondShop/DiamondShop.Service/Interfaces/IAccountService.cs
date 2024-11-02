using DiamondShop.Repository.Pagination;
using DiamondShop.Repository.ViewModels.Request.Account;
using DiamondShop.Repository.ViewModels.Response.Account;

namespace DiamondShop.Service.Interfaces;

public interface IAccountService
{
    Task<Paginate<GetAccountDetailResponse>> GetPagedAccount(QueryAccountRequest? filter, int page, int size);
}