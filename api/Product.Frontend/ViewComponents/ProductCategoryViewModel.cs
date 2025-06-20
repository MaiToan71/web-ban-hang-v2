using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Project.Caching.Interfaces;
using Project.Services.Products.Interfaces;
using Project.ViewModels.Products;
using Project.ViewModels.Products.Manage;
using static Project.Utilities.Constants.SystemConstants;

namespace Product.Frontend.ViewComponents
{

    [ViewComponent(Name = "ProductCategory")]
    public class ProductCategoryViewModel : ViewComponent
    {
        private readonly ILogger<ProductViewModel> _logger;
        private readonly ICachingExtension _ICachingExtension;
        private readonly IProductService _IProductService;
        private readonly AppSettings _AppSettings;

        public ProductCategoryViewModel(ILogger<ProductViewModel> logger,
            IOptions<AppSettings> appSettings, ICachingExtension ICachingExtension,
            IProductService IProductService)
        {
            _ICachingExtension = ICachingExtension;
            _logger = logger;
            _IProductService = IProductService;
            _AppSettings = appSettings.Value;
        }

        public async Task<IViewComponentResult> InvokeAsync(int PostTypeId)
        {
            try
            {
                List<ProductViewModel> _products;
                //string cacheKey = "ProductCategory";
                //if (_ICachingExtension.TryGetCache(out _products, cacheKey))
                //{
                //    _products = _products;
                //}
                //else
                //{
                //    var menuPagingRequest = new GetProductPagingRequest
                //    {
                //        PageIndex = 1,
                //        PageSize = 8,
                //        PostTypeId = PostTypeId,
                //    };
                //    _products = _IProductService.GetAllPaging(menuPagingRequest).Result.Items;
                //    _ICachingExtension.SetCache(cacheKey, _products, 10 * 60);
                //}
                var menuPagingRequest = new GetProductPagingRequest
                {
                    PageIndex = 1,
                    PageSize = 8,
                    PostTypeId = PostTypeId,
                };
                _products = _IProductService.GetAllPaging(menuPagingRequest).Result.Items;
                return View(_products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ViewComponent: {ex.Message}");
            }

            return View(null);
        }
    }
}
