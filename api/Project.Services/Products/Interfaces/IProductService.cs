using Project.ViewModels;
using Project.ViewModels.Products;
using Project.ViewModels.Products.Manage;

namespace Project.Services.Products.Interfaces
{
    public interface IProductService
    {
        Task<int> Create(ProductCreateRequest request, Guid userId);
        Task<int> Creates(dynamic data);
        Task<int> Update(ProductUpdateRequest request, Guid userId);
        Task<int> Delete(int productId);

        Task<ProductViewModel> GetByUrl(string url);
        Task<ProductViewModel> GetById(int productId);
        Task<PagedResult<ProductViewModel>> GetAllPaging(GetProductPagingRequest request);



    }
}
