using DiamondShop.Repository.Enums;
using DiamondShop.Repository.Pagination;
using DiamondShop.Repository.ViewModels.Request.Diamond;
using DiamondShop.Repository.ViewModels.Response.Diamond;

namespace DiamondShop.Service.Interfaces;

public interface IDiamondService
{
    Task<GetDiamondId> CreateDiamond(CreateDiamondRequest createDiamondRequest);
    Task UpdateDiamond(Guid id, UpdateDiamondRequest updateDiamondRequest);
    
    Task<Paginate<GetDiamondPagedResponse>> GetPageDiamonds(int page, int size);
    Task<GetDiamondPagedResponse> GetDiamondDetailsById(Guid id);
    
    Task ChangStatusDiamond(Guid diamondId, DiamondStatus status);
    
}