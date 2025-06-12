using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Project.Caching.Interfaces;
using Project.Services.Menus;
using Project.ViewModels.Menus;
using static Project.Utilities.Constants.SystemConstants;

namespace Product.Frontend.ViewComponents
{
    [ViewComponent(Name = "Menu")]
    public class MenuViewComponent : ViewComponent
    {
        private readonly ILogger<MenuViewComponent> _logger;
        private readonly ICachingExtension _ICachingExtension;
        private readonly IMenusService _IMenusService;
        private readonly AppSettings _AppSettings;

        public MenuViewComponent(ILogger<MenuViewComponent> logger,
            IOptions<AppSettings> appSettings, ICachingExtension ICachingExtension,
            IMenusService menuService)
        {
            _ICachingExtension = ICachingExtension;
            _logger = logger;
            _IMenusService = menuService;
            _AppSettings = appSettings.Value;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                List<MenuViewModel> _menus;
                string cacheKey = "Menus";
                if (_ICachingExtension.TryGetCache(out _menus, cacheKey))
                {
                    _menus = _menus;
                }
                else
                {
                    var menuPagingRequest = new GetMenuPagingRequest
                    {
                        IsPublish = true,
                        PageIndex = 1,
                        PageSize = 1000,
                    };
                    _menus = _IMenusService.GetAllPaging(menuPagingRequest).Result.Items;
                    _ICachingExtension.SetCache(cacheKey, _menus, 10 * 60);
                }
                return View(_menus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ViewComponent: {ex.Message}");
            }

            return View(null);
        }

    }
}
