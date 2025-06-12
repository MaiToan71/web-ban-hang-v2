using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Project.Data.EF;
using Project.Data.Entities;
using Project.Enums;
using Project.Services.Products.Interfaces;
using Project.Services.Storages;
using Project.Utilities.Exceptions;
using Project.ViewModel.PostTypes;
using Project.ViewModels;
using Project.ViewModels.Attributes;
using Project.ViewModels.Images;
using Project.ViewModels.Products;
using Project.ViewModels.Products.Manage;
using System.Net.Http.Headers;

namespace Project.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly ProductDbContext _context;
        private readonly IStorageService _storageService;
        private const string USER_CONTENT_FOLDER_NAME = "UploadFiles";
        public ProductService(ProductDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }
        public async Task<ProductViewModel> GetById(int productId)
        {
            var output = await _context.Products.Include(m => m.PostType)
                .Select(product => new ProductViewModel
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
                    SellingPrice = product.SellingPrice,
                    CapitalPrice = product.CapitalPrice,
                    Images = (List<ImageViewModel>)(from pImage in product.ProductImages
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
            if (output == null) throw new ProductException($"Cannot find a product with id: {productId}");

            if (!string.IsNullOrEmpty(output.Content))
            {
                if (output.Content.ToLower() == "null")
                {
                    output.Content = await FindContent(output.Url);

                }
            }
            var ProductAttributes = await _context.ProductAttributes.Where(m => m.ProductId == productId).ToListAsync();
            var attributes = await _context.Attributes.Where(m => ProductAttributes.Select(m => m.AttributeId).Contains(m.Id))
                .Select(m => new ProductAttributeViewModel
                {
                    Name = m.Name,
                    Description = m.Description,
                    Type = m.Type,
                    AttributeId = m.Id

                })
                .ToListAsync()
                ;
            var outputAtributes = new List<ProductAttributeViewModel>();

            foreach (var pa in ProductAttributes)
            {
                var outputAtribute = new ProductAttributeViewModel()
                {
                    Quantity = pa.Quantity,
                    AttributeId = pa.AttributeId,
                    ProductId = pa.ProductId,
                    Price = pa.Price,
                };
                foreach (var a in attributes)
                {
                    if (pa.AttributeId == a.AttributeId)
                    {
                        outputAtribute.Name = a.Name;
                        outputAtribute.Description = a.Description;
                        outputAtribute.Type = a.Type;
                        outputAtribute.AttributeId = a.AttributeId;
                    }
                }
                outputAtributes.Add(outputAtribute);


            }
            output.ProductAttributes = outputAtributes;

            return output;
        }


        public async Task<ProductViewModel> GetByUrl(string url)
        {
            var output = await _context.Products.Include(m => m.PostType)
                .Select(product => new ProductViewModel
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
                    ExportDate = product.ExportDate,
                    InputDate = product.InputDate,
                    CreateTime = product.CreatedTime,
                    ExpireDate = product.ExpireDate,
                    SellingPrice = product.SellingPrice,
                    CapitalPrice = product.CapitalPrice,
                    Images = (List<ImageViewModel>)(from pImage in product.ProductImages
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
                    var item = _context.Products.FirstOrDefault(m => m.Id == output.Id);
                    item.Content = output.Content;
                    await _context.SaveChangesAsync();

                }
            }
            if (output == null) throw new ProductException($"Cannot find a product with : {url}");
            var Product = output;
            return Product;
        }

        public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetProductPagingRequest request)
        {
            //1. Select join
            var query = _context.Products

                .OrderByDescending(m => m.Id)
                .Where(m => m.IsDeleted == false).AsQueryable();

            // 2.Filter

            if (request.WorkflowId != null)
            {
                query = query.Where(m => m.WorkflowId == request.WorkflowId);

                //if (request.WorkflowId ==request.WorkflowId)
                //{
                //    query = query.Where(m => m.UserId == request.UserId);
                //}
            }
            ;

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.Title.ToLower().Contains(request.Keyword.Trim().ToLower()));
            }
            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(x => x.Title.ToLower().Contains(request.Search.ToLower()));
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
            if (request.SellingPriceFrom != null)
            {
                query = query.Where(m => m.SellingPrice >= (float)request.SellingPriceFrom);
            }
            if (request.SellingPriceTo != null)
            {
                query = query.Where(m => m.SellingPrice <= (float)request.SellingPriceTo);
            }
            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                 .Select(product => new ProductViewModel
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
                     Discount = product.Discount,
                     Quantity = product.Quantity,
                     QuantitySold = product.QuantitySold,
                     ExportDate = product.ExportDate,
                     ExpireDate = product.ExpireDate,
                     InputDate = product.InputDate,
                     Author = product.CreatedUser,
                     SellingPrice = product.SellingPrice,
                     CapitalPrice = product.CapitalPrice,
                     PostTypeId = product.PostTypeId,
                     Images = (List<ImageViewModel>)(from pImage in product.ProductImages.Take(1)
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

            //4 .Select and projecttion
            var pagedResult = new PagedResult<ProductViewModel>()
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
        public async Task<int> CreatePostImage(int productId, PostImageCreateRequest request)
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

            var imageProduct = new ProductImage()
            {
                ProductId = productId,
                ImageId = image.Id
            };
            _context.ProductImages.Add(imageProduct);
            return await _context.SaveChangesAsync();

        }
        public async Task<int> Create(ProductCreateRequest request, Guid userId)
        {
            try
            {
                var user = await _context.AppUsers.FindAsync(userId);
                if (user == null)
                {
                    throw new ProductException($"Cannot find a user");
                }
                var product = new Product()
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
                    PostTypeEnum = request.PostTypeEnum,
                    Quantity = 0,
                    QuantitySold = 0,

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
                _context.Products.Add(product);
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

                        int selectedFilesImage = await this.CreatePostImage(product.Id, selectedFilesImageRequest);
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
                List<Product> Products = new List<Product>();

                var user = _context.AppUsers.FirstOrDefault(m => m.UserName == "ZenAITeam");
                if (user == null)
                {
                    throw new ProductException($"Cannot find a user");
                }

                foreach (var request in data.news_info)
                {


                    var product = new Product()
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




                    Products.Add(product);

                }


                await UpdateData(Products);
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
                var data = _context.Products.Skip((PageIndex - 1) * PageSize)
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

        public async Task<bool> UpdateData(List<Product> Products)
        {
            List<Product> adds = new List<Product>();
            List<Product> updates = new List<Product>();
            foreach (var p in Products)
            {

                var checkProduct = _context.Products.Where(m => m.Url == p.Url && m.IsDeleted == false).ToList();
                if (checkProduct.Count() == 0)
                {
                    p.Content = "Null";
                    adds.Add(p);

                }

            }
            if (adds.Count() > 0)
            {
                _context.Products.AddRange(adds);

            }
            if (updates.Count() > 0)
            {

                _context.Products.UpdateRange(updates);
            }
            _context.SaveChanges();

            return true;
        }
        public bool checkUrl(string url)
        {
            var item = _context.Products.Where(m => m.Url == url).FirstOrDefault();
            if (item != null)
            {
                return true;
            }
            return false;
        }

        public async Task<int> Update(ProductUpdateRequest request, Guid userId)
        {
            var product = await _context.Products.FindAsync(request.Id);

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
            //  product.Quantity = request.Quantity;
            //  product.QuantitySold = request.QuantitySold;
            //  if (product.InputDate != null)
            // {
            product.InputDate = request.InputDate;

            //  }
            // if (product.ExpireDate != null)
            //{
            product.ExpireDate = request.ExpireDate;
            // }
            // if (product.ExportDate != null)
            //{
            product.ExportDate = request.ExportDate;
            //}

            product.SellingPrice = request.SellingPrice;
            product.CapitalPrice = request.CapitalPrice;
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

                    int selectedFilesImage = await this.CreatePostImage(product.Id, selectedFilesImageRequest);
                }
            }

            if (request.Attributes != null)
            {
                await UpdateProductAttributeAsync(product.Id, request.Attributes);
            }

            await _context.SaveChangesAsync();
            return 1;
        }

        public async Task<bool> UpdateProductAttributeAsync(int productId, List<ProductAttributeRequest> attributes)
        {
            var checkProducts = await _context.ProductAttributes.Where(m => m.ProductId == productId).ToListAsync();
            if (checkProducts.Count() > 0)
            {
                _context.RemoveRange(checkProducts);
                await _context.SaveChangesAsync();
            }
            var updates = new List<ProductAttribute>();
            foreach (var attribute in attributes)
            {
                var p = new ProductAttribute
                {
                    Quantity = attribute.Quantity,
                    ProductId = productId,
                    AttributeId = attribute.AttributeId,
                    Price = attribute.Price,
                };
                updates.Add(p);
            }
            _context.AddRange(updates);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> Delete(int productId)
        {
            var item = await _context.Products.FindAsync(productId);
            if (item != null)
            {
                item.IsDeleted = true;
                return await _context.SaveChangesAsync();
            }
            return 0;
        }


    }
}