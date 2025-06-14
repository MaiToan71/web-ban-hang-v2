using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Project.Caching.Interfaces;
using Project.Enums;
using Project.Services.PostTypes;
using Project.ViewModel.PostTypes;
using static Project.Utilities.Constants.SystemConstants;

namespace Product.Frontend.ViewComponents
{

    [ViewComponent(Name = "Categories")]
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly ILogger<MenuViewComponent> _logger;
        private readonly ICachingExtension _ICachingExtension;
        private readonly IPostTypeService _IPostTypeService;
        private readonly AppSettings _AppSettings;

        public CategoriesViewComponent(ILogger<MenuViewComponent> logger,
            IOptions<AppSettings> appSettings, ICachingExtension ICachingExtension,
            IPostTypeService IPostTypeService)
        {
            _ICachingExtension = ICachingExtension;
            _logger = logger;
            _IPostTypeService = IPostTypeService;
            _AppSettings = appSettings.Value;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                List<PostTypeViewModel> _categories;
                //string cacheKey = "Categories";
                //if (_ICachingExtension.TryGetCache(out _categories, cacheKey))
                //{
                //    _categories = _categories;
                //}
                //else
                //{
                //    var request = new GetPostTypePagingRequest
                //    {
                //        PostTypeEnum = PostTypeEnum.Warehouse,
                //        PageIndex = 1,
                //        PageSize = 5,
                //    };
                //    _categories = _IPostTypeService.GetAllPaging(request).Result.Items.ToList();
                //    _ICachingExtension.SetCache(cacheKey, _categories, 10 * 60);
                //}
                var request = new GetPostTypePagingRequest
                {
                    PostTypeEnum = PostTypeEnum.Warehouse,
                    PageIndex = 1,
                    PageSize = 5,
                };
                _categories = _IPostTypeService.GetAllPaging(request).Result.Items.ToList();
                // _ICachingExtension.SetCache(cacheKey, _categories, 10 * 60);
                return View(_categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ViewComponent: {ex.Message}");
            }

            return View(null);
        }
    }
}
