using Microsoft.AspNetCore.Mvc;
using Project.Caching.Interfaces;
using Project.Services.Images;
using Project.Services.Products.Interfaces;
using Project.Services.Users.Interfaces;
using Project.ViewModels;
using Project.ViewModels.Products.Manage;

namespace Project.API.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IAppUserService _IUserService;
        private readonly ICachingExtension _ICachingExtension;
        public readonly IProductService _IProductService;
        private readonly ILogger<ProductsController> _logger;
        private readonly IImageService _IImageService;
        public ProductsController(IProductService IProductService, ILogger<ProductsController> logger, IImageService IImageService, IAppUserService userService,
            ICachingExtension cachingExtension) : base(cachingExtension, userService)
        {
            _IProductService = IProductService;
            _logger = logger;
            _IImageService = IImageService;
        }
        [Route("search")]
        [HttpPost]
        public async Task<ActionResult> Searching([FromBody] GetProductPagingRequest request)
        {
            try
            {
                request.UserId = CurrentUserViewModel.UserId;
                var data = await _IProductService.GetAllPaging(request);

                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult> GetById(int id)
        {
            var PostResult = new PostResult();
            try
            {
                var data = await _IProductService.GetById(id);
                if (data == null)
                {
                    return BadRequest("Cannot find product");
                }
                PostResult.Data = data;
            }
            catch (Exception ex)
            {
                PostResult.Status = false;
                PostResult.Data = ex.Message;
            }
            return Ok(PostResult);
        }

        [Route("add")]
        [HttpPost]
        public async Task<ActionResult> AddNew([FromForm] ProductCreateRequest request)
        {
            var PostResult = new PostResult();
            try
            {

                //  request.UserId = userId;
                var productId = await _IProductService.Create(request, CurrentUserViewModel.UserId);
                PostResult.Data = productId;
            }
            catch (Exception ex)
            {
                PostResult.Status = false;
                PostResult.Data = ex.Message;
            }
            return Ok(PostResult);
        }

        [Route("update")]
        [HttpPut]
        public async Task<ActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            var PostResult = new PostResult();
            try
            {
                var productId = await _IProductService.Update(request, CurrentUserViewModel.UserId);

            }
            catch (Exception ex)
            {
                PostResult.Status = false;
                PostResult.Data = ex.Message;
            }
            return Ok(PostResult);
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteById(int id)
        {
            var PostResult = new PostResult();
            try
            {
                var data = await _IProductService.Delete(id);

            }
            catch (Exception ex)
            {
                PostResult.Status = false;
                PostResult.Data = ex.Message;
            }
            return Ok(PostResult);
        }
        [Route("image")]
        [HttpDelete]
        public async Task<ActionResult> DeleteImage(int imageId, int productId)
        {
            var PostResult = new PostResult();
            try
            {
                var data = await _IImageService.DeleteImage(imageId, productId);
                if (data == null)
                {
                    return BadRequest("Cannot find product");
                }
            }
            catch (Exception ex)
            {
                PostResult.Status = false;
                PostResult.Data = ex.Message;
            }
            return Ok(PostResult);
        }

    }
}
