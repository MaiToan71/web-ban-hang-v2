using Project.ViewModels;
using Project.ViewModels.Posts;
using Project.ViewModels.Posts.Manage;

namespace Project.Services.Posts.Interfaces
{
    public interface IPostService
    {
        Task<int> Create(PostCreateRequest request, Guid userId);
        Task<int> Creates(dynamic data);
        Task<int> Update(PostUpdateRequest request, Guid userId);
        Task<int> Delete(int productId);

        Task<PostViewModel> GetByUrl(string url);
        Task<PostViewModel> GetById(int productId);
        Task<PagedResult<PostViewModel>> GetAllPaging(GetPostPagingRequest request);



    }
}
