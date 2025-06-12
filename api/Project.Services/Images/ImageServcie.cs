using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Project.Data.EF;
using Project.Data.Entities;
using Project.Services.Storages;
using Project.ViewModel.PostTypes;
using Project.ViewModels;
using Project.ViewModels.Images;
using System.Net.Http.Headers;

namespace Project.Services.Images
{
    public class ImageServcie : IImageService
    {
        private readonly ProductDbContext _context;
        private readonly IStorageService _storageService;
        private const string USER_CONTENT_FOLDER_NAME = "UploadFiles";
        public ImageServcie(ProductDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return "/" + USER_CONTENT_FOLDER_NAME + "/" + fileName;
        }

        public async Task<int> CreateImage(ImageCreateRequest request, Guid CreatedUserId, string CreatedUser)
        {
            var image = new Data.Entities.Image()
            {

                Caption = request.Title,
                IsDefault = false,
                IsBanner = false,
                DateCreated = DateTime.Now,
                SortOrder = 1,
                CreatedId = CreatedUserId,
                CreatedUser = CreatedUser,
                ModifiedId = CreatedUserId,
                ModifiedUser = CreatedUser

            };
            if (request.ImageFile != null)
            {
                image.ImagePath = await this.SaveFile(request.ImageFile);
                image.FileSize = request.ImageFile.Length;
            }
            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            List<PostTypeImage> PostTypeImages = new List<PostTypeImage>();
            foreach (var i in request.CategoryIds)
            {
                var postTypeImage = new PostTypeImage()
                {
                    PostTypeId = i,
                    ImageId = image.Id
                };
                PostTypeImages.Add(postTypeImage);
            }

            await _context.PostTypeImages.AddRangeAsync(PostTypeImages);
            await _context.SaveChangesAsync();
            return 1;
        }

        public async Task<int> DeleteImage(int imageId)
        {
            var image = _context.Images.FirstOrDefault(m => m.Id == imageId);
            if (image != null)
            {
                image.IsDeleted = true;
            }
            await _context.SaveChangesAsync();
            return 1;

        }

        public async Task<dynamic> GetStatistics(GetImagePagingRequest request)
        {

            var categories = await _context.PostTypes.Where(m => m.IsDeleted == false && m.PostTypeEnum == Enums.PostTypeEnum.Folder).ToListAsync();
            List<dynamic> lists = new List<dynamic>();
            foreach (var category in categories)
            {
                var files = await _context.PostTypeImages.Where(m => m.PostTypeId == category.Id).ToListAsync();
                var fileSize = await _context.Images.Where(m => files.Select(f => f.ImageId).Contains(m.Id)).Select(f => f.FileSize).SumAsync();
                var cate = new
                {
                    Name = category.Name,
                    TotalFile = files.Count(),
                    TotalFilesize = fileSize
                };
                lists.Add(cate);

            }

            return lists;

        }

        public async Task<PagedResult<ImageViewModel>> GetAllPaging(GetImagePagingRequest request)
        {

            //1. Danh sách ảnh
            var query = _context.Images.Where(m => m.IsDeleted == false).AsQueryable();

            //2. Lọc theo điều kiện
            if (request.CategoryId != null)
            {
                query = query.Where(m => m.PostTypeImages.Any(m => m.PostTypeId == request.CategoryId));
            }

            //3. Phân trang
            int total = query.Count();
            var data = await query.OrderByDescending(m => m.DateCreated).Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
              .Select(item => new ImageViewModel
              {
                  Id = item.Id,
                  Caption = item.Caption,
                  DateCreated = item.DateCreated,
                  FileSize = item.FileSize,
                  ImagePath = item.ImagePath,
                  CreatedUser = item.CreatedUser,
                  CreatedId = item.CreatedId,
                  IsDefault = false,
                  SortOrder = item.SortOrder,
                  Categories = (List<PostTypeViewModel>)(from c in item.PostTypeImages
                                                         select new PostTypeViewModel
                                                         {
                                                             Id = c.PostTypeId,
                                                             Name = c.PostType.Name
                                                         })

              })
                .ToListAsync();


            //4 .Select and projecttion
            var pagedResult = new PagedResult<ImageViewModel>()
            {
                TotalRecord = total,
                Items = data,
            };

            return pagedResult;

        }

        public async Task<ImageViewModel> GetById(int id)
        {
            var response = await _context.Images.Select(item => new ImageViewModel
            {
                Id = item.Id,
                Caption = item.Caption,
                DateCreated = item.DateCreated,
                ImagePath = item.ImagePath,
                FileSize = item.FileSize,
                IsDefault = item.IsDefault,
                SortOrder = item.SortOrder,
                Categories = (List<PostTypeViewModel>)(from c in item.PostTypeImages
                                                       select new PostTypeViewModel
                                                       {
                                                           Id = c.PostTypeId,
                                                           Name = c.PostType.Name
                                                       })
            }).FirstOrDefaultAsync(m => m.Id == id);

            return response;
        }

        private async Task<bool> RemoveCategoryImage(int imageId)
        {
            var categoryImages = _context.PostTypeImages.Where(m => m.ImageId == imageId);
            List<PostTypeImage> removeList = new List<PostTypeImage>();
            foreach (var i in categoryImages)
            {

                removeList.Add(i);
            }
            _context.RemoveRange(removeList);
            await _context.SaveChangesAsync();
            return true;

        }


        public async Task<int> UpdateImage(ImageCreateRequest request)
        {
            var image = await _context.Images.FirstOrDefaultAsync(m => m.Id == request.Id);
            if (image == null)
            {
                return 0;
            }
            image.Caption = request.Title;

            if (request.ImageFile != null)
            {
                image.ImagePath = await this.SaveFile(request.ImageFile);
                image.FileSize = (long)request.ImageFile.Length;
            }
            await _context.SaveChangesAsync();
            await RemoveCategoryImage(image.Id);

            List<PostTypeImage> CategoryImages = new List<PostTypeImage>();
            foreach (var i in request.CategoryIds)
            {
                var categoryImage = new PostTypeImage()
                {
                    PostTypeId = i,
                    ImageId = image.Id
                };
                CategoryImages.Add(categoryImage);
            }
            await _context.PostTypeImages.AddRangeAsync(CategoryImages);
            await _context.SaveChangesAsync();
            return 1;

        }




        private async Task<int> RemoveImage(string filename)
        {
            await _storageService.DeleteFileAsync(filename);
            return 1;
        }


        public async Task<bool> RemoveTableImage(List<int> Ids)
        {
            var items = _context.Images.Where(m => Ids.Any(i => i == m.Id)).ToList();
            _context.Images.RemoveRange(items);
            _context.SaveChanges();
            return true;
        }



        public async Task<bool> RemoveProductImageByCustomerId(int imageId, int productId)
        {
            var products = await _context.ProductImages.Include(i => i.Images).Where(m => m.ProductId == productId && m.ImageId == imageId).ToListAsync();
            if (products.Count() == 0)
            {
                return false;
            }
            List<int> imageIds = new List<int>();
            foreach (var image in products)
            {
                imageIds.Add(image.ImageId);
                var fileName = image.Images.ImagePath.Replace($"/{USER_CONTENT_FOLDER_NAME}/", "");
                await RemoveImage(fileName);
            }
            _context.ProductImages.RemoveRange(products);
            await _context.SaveChangesAsync();

            await RemoveTableImage(imageIds);
            return true;
        }

        public async Task<bool> RemovePostImageByCustomerId(int imageId, int postId)
        {
            var posts = await _context.PostImages.Include(i => i.Images).Where(m => m.PostId == postId && m.ImageId == imageId).ToListAsync();
            if (posts.Count() == 0)
            {
                return false;
            }
            List<int> imageIds = new List<int>();
            foreach (var image in posts)
            {
                imageIds.Add(image.ImageId);
                var fileName = image.Images.ImagePath.Replace($"/{USER_CONTENT_FOLDER_NAME}/", "");
                await RemoveImage(fileName);
            }
            _context.PostImages.RemoveRange(posts);
            await _context.SaveChangesAsync();

            await RemoveTableImage(imageIds);
            return true;
        }

        public async Task<int> DeleteImage(int imageId, int productId)
        {
            await RemoveProductImageByCustomerId(imageId, productId);

            return 1;
        }

        public async Task<int> DeletePostImage(int imageId, int postId)
        {
            await RemovePostImageByCustomerId(imageId, postId);

            return 1;
        }
    }
}
