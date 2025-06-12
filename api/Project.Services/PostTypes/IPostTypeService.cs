using Project.ViewModel.PostTypes;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.PostTypes
{
    public interface  IPostTypeService
    {
        Task<PagedResult<PostTypeViewModel>> GetAllPaging(GetPostTypePagingRequest request);
        Task<PostTypeViewModel> FindById(int Id);
        Task<int> Add(PostTypeCreateRequest request);
        Task<int> Delete(int Id);
        Task<int> Update(PostTypeUpdateRequest request);
    }
}
