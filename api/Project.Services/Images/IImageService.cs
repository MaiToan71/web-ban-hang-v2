using Project.ViewModels;
using Project.ViewModels.Images;

namespace Project.Services.Images
{
    public interface IImageService
    {
        Task<int> UpdateImage(ImageCreateRequest request);
        Task<ImageViewModel> GetById(int id);

        Task<int> CreateImage(ImageCreateRequest request, Guid CreatedUserId, string CreatedUser);
        Task<int> DeleteImage(int imageId);
        Task<PagedResult<ImageViewModel>> GetAllPaging(GetImagePagingRequest request);

        Task<int> DeleteImage(int imageId, int productId);

        Task<dynamic> GetStatistics(GetImagePagingRequest request);

        Task<int> DeletePostImage(int imageId, int postId);
    }
}
