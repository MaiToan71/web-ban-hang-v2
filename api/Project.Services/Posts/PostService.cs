using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Project.Data.EF;
using Project.Data.Entities;
using Project.Enums;
using Project.Services.Posts.Interfaces;
using Project.Services.Storages;
using Project.Utilities.Exceptions;
using Project.ViewModel.PostTypes;
using Project.ViewModels;
using Project.ViewModels.Images;
using Project.ViewModels.Posts;
using Project.ViewModels.Posts.Manage;
using System.Net.Http.Headers;

namespace Project.Services.Posts
{
    public class PostService : IPostService
    {
        private readonly ProductDbContext _context;
        private readonly IStorageService _storageService;
        private const string USER_CONTENT_FOLDER_NAME = "UploadFiles";
        public PostService(ProductDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }
        public async Task<PostViewModel> GetById(int productId)
        {
            var output = await _context.Posts.Include(m => m.PostType)
                .Select(product => new PostViewModel
                {
                    Id = product.Id,
                    Title = product.Title,
                    SubTitle = product.SubTitle,
                    Url = product.Url,
                    Platform = product.Platform,
                    Content = product.Content,
                    IsPublished = product.IsPublished,
                    IsShowHome = product.IsShowHome,
                    WorkflowId = product.WorkflowId,
                    ShortContent = product.ShortContent,
                    CoverImage = product.CoverImage,
                    Description = product.Description,
                    Source = product.Source,
                    Quantity = product.Quantity,
                    QuantitySold = product.QuantitySold,
                    ExportDate = product.ExportDate,
                    InputDate = product.InputDate,
                    ExpireDate = product.ExpireDate,
                    PostTypeId = product.PostTypeId,
                    Images = (List<ImageViewModel>)(from pImage in product.PostImages
                                                    select new ImageViewModel
                                                    {
                                                        FileSize = pImage.Images.FileSize,
                                                        Id = pImage.ImageId,
                                                        Caption = pImage.Images.Caption,
                                                        ImagePath = pImage.Images.ImagePath,
                                                    }).OrderByDescending(i => i.Id),
                    PostType = new PostTypeViewModel
                    {
                        Id = product.PostTypeId,
                        Name = product.PostType.Name,
                        Description = product.PostType.Description,
                        SortOrder = product.PostType.SortOrder,
                        IsShowOnHome = product.PostType.IsShowOnHome,
                        ParentId = product.PostType.ParentId,
                        IsDelete = product.PostType.IsDeleted,
                        Status = product.PostType.Status,
                    }

                }).FirstOrDefaultAsync(m => m.Id == productId);

            if (!string.IsNullOrEmpty(output.Content))
            {
                if (output.Content.ToLower() == "null")
                {
                    output.Content = await FindContent(output.Url);

                }
            }
            var departmentids = await _context.PostDepartments.Where(m => m.PostId == productId).ToListAsync();
            var departmemts = await _context.Departments.Where(m => departmentids.Select(m => m.DeparmentId).Contains(m.Id)).ToListAsync();
            output.Departments = departmemts;

            if (output == null) throw new ProductException($"Cannot find a product with id: {productId}");

            return output;
        }


        public async Task<PostViewModel> GetByUrl(string url)
        {
            var output = await _context.Posts.Include(m => m.PostType)
                .Select(product => new PostViewModel
                {
                    Id = product.Id,
                    Title = product.Title,
                    SubTitle = product.SubTitle,
                    Url = product.Url,
                    Content = product.Content,
                    IsPublished = product.IsPublished,
                    IsShowHome = product.IsShowHome,
                    WorkflowId = product.WorkflowId,
                    ShortContent = product.ShortContent,
                    CoverImage = product.CoverImage,
                    Description = product.Description,
                    Source = product.Source,
                    PostTypeId = product.PostTypeId,
                    Quantity = product.Quantity,
                    QuantitySold = product.QuantitySold,
                    CreateUserId = product.CreatedId,
                    Author = product.CreatedUser,
                    ExportDate = product.ExportDate,
                    InputDate = product.InputDate,
                    CreateTime = product.CreatedTime,
                    ExpireDate = product.ExpireDate,
                    Images = (List<ImageViewModel>)(from pImage in product.PostImages
                                                    select new ImageViewModel
                                                    {
                                                        FileSize = pImage.Images.FileSize,
                                                        Id = pImage.ImageId,
                                                        Caption = pImage.Images.Caption,
                                                        ImagePath = pImage.Images.ImagePath,
                                                    }).OrderByDescending(i => i.Id),
                    PostType = new PostTypeViewModel
                    {
                        Id = product.PostTypeId,
                        Name = product.PostType.Name,
                        Description = product.PostType.Description,
                        SortOrder = product.PostType.SortOrder,
                        IsShowOnHome = product.PostType.IsShowOnHome,
                        ParentId = product.PostType.ParentId,
                        IsDelete = product.PostType.IsDeleted,
                        Status = product.PostType.Status,
                    }

                })
              .FirstOrDefaultAsync(m => m.Url == url);
            if (!string.IsNullOrEmpty(output.Content))
            {
                if (output.Content.ToLower() == "null")
                {
                    output.Content = await FindContent(output.Url);
                    var item = _context.Posts.FirstOrDefault(m => m.Id == output.Id);
                    item.Content = output.Content;
                    await _context.SaveChangesAsync();

                }
            }
            var user = await _context.AppUsers.Where(m => m.Id == output.CreateUserId).FirstOrDefaultAsync();
            if (user != null)
            {
                output.Author = user.FullName;
            }
            if (output == null) throw new ProductException($"Cannot find a product with : {url}");
            var post = output;
            return post;
        }

        public async Task<PagedResult<PostViewModel>> GetAllPaging(GetPostPagingRequest request)
        {
            //1. Select join
            var query = _context.Posts

                .OrderByDescending(m => m.Id)
                .Where(m => m.IsDeleted == false).AsQueryable();

            // 2.Filter

            if (request.WorkflowId != null)
            {
                query = query.Where(m => m.WorkflowId == request.WorkflowId);

                if (request.WorkflowId == Workflow.CREATED)
                {
                    query = query.Where(m => m.UserId == request.UserId);
                }
            }
            ;

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.Title.Contains(request.Keyword));
            }
            if (request.PostTypeId != null)
            {
                query = query.Where(m => m.PostTypeId == request.PostTypeId);
            }

            if (request.IsPublished != null)
            {
                query = query.Where(m => m.IsPublished == request.IsPublished);
            }
            if (request.IsShowHome != null)
            {
                query = query.Where(m => m.IsShowHome == request.IsShowHome);
            }
            if (request.PostTypeEnum != null)
            {
                query = query.Where(m => m.PostTypeEnum == (Enums.PostTypeEnum)request.PostTypeEnum);
            }

            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                 .Select(product => new PostViewModel
                 {
                     Id = product.Id,
                     Title = product.Title,
                     SubTitle = product.SubTitle,
                     Url = product.Url,
                     //  Content = product.Content,
                     IsPublished = product.IsPublished,
                     IsShowHome = product.IsShowHome,
                     //  WorkflowId = product.WorkflowId,
                     Platform = product.Platform,
                     ShortContent = product.ShortContent,
                     //   CoverImage = product.CoverImage,

                     Description = product.Description,
                     //    Source = product.Source,
                     CreateTime = product.CreatedTime,
                     Quantity = product.Quantity,
                     QuantitySold = product.QuantitySold,
                     ExportDate = product.ExportDate,
                     ExpireDate = product.ExpireDate,
                     InputDate = product.InputDate,
                     Author = product.CreatedUser,
                     PostTypeId = product.PostTypeId,
                     Images = (List<ImageViewModel>)(from pImage in product.PostImages.Take(1)
                                                     select new ImageViewModel
                                                     {
                                                         FileSize = pImage.Images.FileSize,
                                                         Id = pImage.ImageId,
                                                         Caption = pImage.Images.Caption,
                                                         ImagePath = pImage.Images.ImagePath,
                                                     }).OrderByDescending(i => i.Id),
                     PostType = new PostTypeViewModel
                     {
                         Id = product.PostTypeId,
                         Name = product.PostType.Name,
                     }
                 })
                .ToListAsync();
            var departmentids = await _context.PostDepartments.Where(m => data.Select(d => d.Id).Contains(m.PostId)).ToListAsync();
            var departmemts = await _context.Departments.Where(m => departmentids.Select(m => m.DeparmentId).Contains(m.Id)).ToListAsync();
            foreach (var d in data)
            {
                var checkDepartmemtIds = departmentids.Where(m => m.PostId == d.Id).ToList();
                var checkDepatments = departmemts.Where(m => checkDepartmemtIds.Select(x => x.DeparmentId).Contains(m.Id)).ToList();
                if (checkDepatments != null)
                {
                    d.Departments = checkDepatments.ToList();

                }
            }
            //4 .Select and projecttion
            var pagedResult = new PagedResult<PostViewModel>()
            {
                TotalRecord = totalRow,
                Items = data,
            };
            return pagedResult;
        }
        private async Task<string> SaveImage(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return "/" + USER_CONTENT_FOLDER_NAME + "/" + fileName;
        }
        public async Task<int> CreateProductImage(int productId, PostImageCreateRequest request)
        {
            var image = new Image()
            {

                Caption = request.Caption,
                IsDefault = request.IsDefault,
                DateCreated = DateTime.Now,
                SortOrder = request.SortOrder,

            };
            image.ImagePath = request.FilePath;
            image.FileSize = request.FileSize;
            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            var imageProduct = new PostImage()
            {
                PostId = productId,
                ImageId = image.Id
            };
            _context.PostImages.Add(imageProduct);
            return await _context.SaveChangesAsync();

        }
        public async Task<int> Create(PostCreateRequest request, Guid userId)
        {
            try
            {
                var user = await _context.AppUsers.FindAsync(userId);
                if (user == null)
                {
                    throw new ProductException($"Cannot find a user");
                }
                var product = new Post()
                {
                    Title = request.Title,
                    SubTitle = request.SubTitle,
                    Url = request.Url,
                    Description = request.Description,
                    Content = request.Content,
                    ShortContent = request.ShortContent,
                    Source = request.Source,
                    CoverImage = request.CoverImage,
                    WorkflowId = request.WorkflowId,
                    IsPublished = request.IsPublished,
                    IsShowHome = request.IsShowHome,
                    CreatedTime = DateTime.Now,
                    ModifiedTime = DateTime.Now,
                    PostTypeId = request.PostTypeId,
                    UserId = userId,
                    CreatedUser = user.FullName,
                    Platform = request.Platform,
                    PostTypeEnum = request.PostTypeEnum

                };
                if (string.IsNullOrEmpty(request.Url))
                {
                    product.Url = Project.Utilities.Helpers.Slug.ConvertStringToSlug(request.Title);
                }
                else
                {
                    product.Url = Project.Utilities.Helpers.Slug.ConvertStringToSlug(request.Url);
                }

                if (checkUrl(product.Url))
                {
                    product.Url = product.Url + $"-{DateTimeOffset.Now.ToUnixTimeMilliseconds()}";
                }
                _context.Posts.Add(product);
                int productId = await _context.SaveChangesAsync();
                if (productId > 0)
                {

                    if (request.SelectedFilesImage != null)
                    {
                        var selectedFilesImageRequest = new PostImageCreateRequest
                        {

                            FileSize = (long)request.SelectedFilesImage.Length,
                            FilePath = await this.SaveImage(request.SelectedFilesImage),
                            Caption = request.SelectedFilesImage.FileName,
                            IsDefault = true,
                            SortOrder = 1,
                        };

                        int selectedFilesImage = await this.CreateProductImage(product.Id, selectedFilesImageRequest);
                    }
                }

                if (request.Departments != null)
                {

                    if (request.Departments.Count() > 0)
                    {
                        var inputPostDepartments = new List<PostDepartment>();
                        foreach (var d in request.Departments)
                        {
                            var dInput = new PostDepartment
                            {
                                PostId = product.Id,
                                DeparmentId = d
                            };
                            inputPostDepartments.Add(dInput);

                        }
                        _context.PostDepartments.AddRange(inputPostDepartments);
                        _context.SaveChanges();
                    }
                }

                return product.Id;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public async Task<int> Creates(dynamic data)
        {
            try
            {
                List<Post> posts = new List<Post>();

                var user = _context.AppUsers.FirstOrDefault(m => m.UserName == "ZenAITeam");
                if (user == null)
                {
                    throw new ProductException($"Cannot find a user");
                }

                foreach (var request in data.news_info)
                {


                    var product = new Post()
                    {
                        Title = request.news_title,
                        Url = request.slug,
                        Content = "null",
                        Source = request.industry,
                        CoverImage = "",
                        WorkflowId = Workflow.COMPLETED,
                        IsPublished = true,
                        IsShowHome = true,
                        CreatedTime = DateTime.Now,
                        ModifiedTime = DateTime.Now,
                        PostTypeId = 1,
                        UserId = user.Id,
                        CreatedUser = user.FullName,
                        ShortContent = "null",
                        SubTitle = "null",
                        Description = "null"

                    };
                    string news_short_content = request.news_short_content;
                    if (!string.IsNullOrEmpty(news_short_content))
                    {
                        product.SubTitle = news_short_content;
                        product.Description = news_short_content;
                        product.ShortContent = news_short_content;
                    }
                    else
                    {
                        product.SubTitle = request.news_title;
                        product.Description = request.news_title;
                        product.ShortContent = request.news_title;
                    }




                    posts.Add(product);

                }


                await UpdateData(posts);
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        public async Task<bool> UpdateDataContent()
        {
            try
            {
                int PageIndex = 1;
                int PageSize = 20;
                var data = _context.Posts.Skip((PageIndex - 1) * PageSize)
                     .Take(PageSize);
                _context.Dispose();
                foreach (var i in data.ToList())
                {
                    i.Content = await FindContent(i.Url);
                    await _context.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<string> FindContent(string slug)
        {
            var client = new HttpClient();
            var requestApi = new HttpRequestMessage(HttpMethod.Get, "https://ai.vietcap.com.vn/api/news_from_slug?slug=" + slug);
            var response = await client.SendAsync(requestApi);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            if (response.IsSuccessStatusCode)
            {
                dynamic res = await response.Content.ReadAsStringAsync();
                dynamic r = JsonConvert.DeserializeObject(res);

                return r.news_full_content;

            }
            else
            {
                return "";
            }

        }

        public async Task<bool> UpdateData(List<Post> posts)
        {
            List<Post> adds = new List<Post>();
            List<Post> updates = new List<Post>();
            foreach (var p in posts)
            {

                var checkPost = _context.Posts.Where(m => m.Url == p.Url && m.IsDeleted == false).ToList();
                if (checkPost.Count() == 0)
                {
                    p.Content = "Null";
                    adds.Add(p);

                }

            }
            if (adds.Count() > 0)
            {
                _context.Posts.AddRange(adds);

            }
            if (updates.Count() > 0)
            {

                _context.Posts.UpdateRange(updates);
            }
            _context.SaveChanges();

            return true;
        }
        public bool checkUrl(string url)
        {
            var item = _context.Posts.Where(m => m.Url == url).FirstOrDefault();
            if (item != null)
            {
                return true;
            }
            return false;
        }

        public async Task<int> Update(PostUpdateRequest request, Guid userId)
        {
            var product = await _context.Posts.FindAsync(request.Id);

            if (product == null) throw new ProductException($"Cannot find a product with id: {request.Id}");
            product.Title = request.Title;
            product.SubTitle = request.SubTitle;
            product.Url = Project.Utilities.Helpers.Slug.ConvertStringToSlug(request.Url);
            product.Description = request.Description;
            product.Content = request.Content;
            product.ShortContent = request.ShortContent;
            product.Source = request.Source;
            product.CoverImage = request.CoverImage;
            product.WorkflowId = request.WorkflowId;
            product.IsPublished = request.IsPublished;
            product.IsShowHome = request.IsShowHome;
            product.PostTypeId = request.PostTypeId;
            product.Platform = request.Platform;
            product.ModifiedTime = DateTime.Now;

            //     product.SellingPrice = request.SellingPrice;
            //    product.CapitalPrice = request.CapitalPrice;
            if (checkUrl(product.Url))
            {
                product.Url = product.Url + $"-{DateTimeOffset.Now.ToUnixTimeMilliseconds()}";
            }
            if (request.SelectedFilesImages != null)
            {
                foreach (var SelectedFilesImage in request.SelectedFilesImages)
                {
                    var selectedFilesImageRequest = new PostImageCreateRequest
                    {

                        FileSize = (long)SelectedFilesImage.Length,
                        FilePath = await this.SaveImage(SelectedFilesImage),
                        Caption = SelectedFilesImage.FileName,
                        IsDefault = true,
                        SortOrder = 1,
                    };

                    int selectedFilesImage = await this.CreateProductImage(product.Id, selectedFilesImageRequest);
                }

            }

            if (request.Departments != null)
            {
                var deparmentPost = _context.PostDepartments.Where(m => m.PostId == request.Id).ToList();
                if (deparmentPost.Count() > 0)
                {
                    _context.PostDepartments.RemoveRange(deparmentPost);
                    _context.SaveChanges();
                }
                if (request.Departments.Count() > 0)
                {
                    var inputPostDepartments = new List<PostDepartment>();
                    foreach (var d in request.Departments)
                    {
                        var dInput = new PostDepartment
                        {
                            PostId = request.Id,
                            DeparmentId = d
                        };
                        inputPostDepartments.Add(dInput);

                    }
                    _context.PostDepartments.AddRange(inputPostDepartments);
                    _context.SaveChanges();
                }
            }

            await _context.SaveChangesAsync();
            return 1;
        }

        public async Task<int> Delete(int productId)
        {
            var item = await _context.Posts.FindAsync(productId);
            if (item != null)
            {
                item.IsDeleted = true;
                return await _context.SaveChangesAsync();
            }
            return 0;
        }


    }
}