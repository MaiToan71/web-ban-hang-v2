using Microsoft.AspNetCore.Mvc;
using Project.Caching.Interfaces;
using Project.Services.Images;
using Project.Services.Products.Interfaces;
using Project.Services.Users.Interfaces;
using Project.ViewModels.Products.Manage;

namespace Project.API.Controllers.Clients
{
    [Route("api/client/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IAppUserService _IUserService;
        private readonly ICachingExtension _ICachingExtension;
        public readonly IProductService _IProductService;
        private readonly ILogger<ProductsController> _logger;
        private readonly IImageService _IImageService;
        public ProductsController(IProductService IProductService, ILogger<ProductsController> logger, IImageService IImageService, IAppUserService userService,
            ICachingExtension cachingExtension)
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
                if (request.PageSize > 100)
                {
                    request.PageSize = 100;
                }
                var data = await _IProductService.GetAllPaging(request);

                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
