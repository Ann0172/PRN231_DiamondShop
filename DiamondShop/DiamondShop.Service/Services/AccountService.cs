using System.Linq.Expressions;
using DiamondShop.Repository;
using DiamondShop.Repository.Interfaces;
using DiamondShop.Repository.Models;
using DiamondShop.Repository.Pagination;
using DiamondShop.Repository.ViewModels.Request.Account;
using DiamondShop.Repository.ViewModels.Response.Account;
using DiamondShop.Service.Extensions;
using DiamondShop.Service.Interfaces;
using Mapster;

namespace DiamondShop.Service.Services;

public class AccountService : IAccountService
{
    private readonly IUnitOfWork<Prn231DiamondShopContext> _unitOfWork;

    public AccountService(IUnitOfWork<Prn231DiamondShopContext> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Paginate<GetAccountDetailResponse>> GetPagedAccount(QueryAccountRequest? filter, int page, int size)
    {
        var account = await _unitOfWork.GetRepository<Account>().GetPagingListAsync(page: page, size: size, predicate: ApplyAccountFilter(filter));
        return account.Adapt<Paginate<GetAccountDetailResponse>>();
    }
    
    private Expression<Func<Account, bool>> ApplyAccountFilter(QueryAccountRequest? filter)
    {
        Expression<Func<Account, bool>> filterQuery = account => true;

        if (filter is null) return filterQuery;
        if (filter.Role.HasValue)
        {
            filterQuery = filterQuery.AndAlso(account => account.Role == filter.Role.ToString());
        }
        if (!string.IsNullOrWhiteSpace(filter.Email))
        {
            filterQuery = filterQuery.AndAlso(account => account.Email.Contains(filter.Email));
        }
        if (!string.IsNullOrWhiteSpace(filter.UserName))
        {
            filterQuery = filterQuery.AndAlso(account => account.Username.ToLower().Contains(filter.UserName.ToLower()));
        }
        
        return filterQuery;
    }
}