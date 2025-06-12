using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services
{
    public interface IBaseService<TEnity, TEntityViewModel, GetTEntityPagingRequest, TEntityCreateRequest, TEntityUpdateRequest>
    {
        Task<PagedResult<TEntityViewModel>> GetAllPaging(GetTEntityPagingRequest request);
        Task<TEntityViewModel> FindById(int id);
        Task<int> Add(TEntityCreateRequest request, Guid userId, string fullname);
        Task<int> Delete(int Id);
        Task<int> Update(TEntityUpdateRequest request, Guid userId, string fullname);

    }
}
