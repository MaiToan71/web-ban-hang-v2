using Microsoft.AspNetCore.Mvc;
using Project.Enums;
using Project.Services.Configs;
using Project.Services.Posts.Interfaces;
using Project.Services.PostTypes;
using Project.Services.Products.Interfaces;
using Project.ViewModel.PostTypes;
using Project.ViewModels.Configs;
using Project.ViewModels.Posts.Manage;
using Project.ViewModels.Products.Manage;

namespace Product.Frontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _product;
        private readonly IPostService _post;
        private readonly IConfigService _IConfigService;
        private readonly IPostTypeService _postType;
        public HomeController(ILogger<HomeController> logger, IProductService product, IPostService post,
            IConfigService IConfigService, IPostTypeService posttype)
        {
            _logger = logger;
            _product = product;
            _IConfigService = IConfigService;
            _post = post;
            _postType = posttype;
        }

        public async Task<IActionResult> Index()
        {


            IConfigurationRoot configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            ViewBag.DomainApi = configurationBuilder["Domain:Api"];
            ViewBag.Image = configurationBuilder["Domain:Api"] + "/UploadFiles/e383dfcf-7bc0-4cf9-a470-cf1456611359.jpg";



            var bannerRequest = new GetConfigPagingRequest()
            {
                PageIndex = 1,
                PageSize = 3,
                ConfigEnum = ConfigEnum.BannerHome,
            };
            var banners = await _IConfigService.GetAllPaging(request: bannerRequest);
            ViewBag.Banners = banners.Items;

            bannerRequest.ConfigEnum = ConfigEnum.ProductHome;
            bannerRequest.PageSize = 8;
            var productHotIds = await _IConfigService.GetAllPaging(request: bannerRequest);
            var listIds = productHotIds.Items.Select(m => m.ConfigId).ToList();


            var request = new GetProductPagingRequest()
            {
                PageIndex = 1,
                PageSize = 8,
                ProductIds = listIds,
            };
            var productHots = await _product.GetAllPaging(request);
            ViewBag.ProductHots = productHots.Items;
            var post = await _post.GetAllPaging(new GetPostPagingRequest
            {
                PageIndex = 1,
                PageSize = 3,
                WorkflowId = Workflow.COMPLETED
            });
            ViewBag.Posts = post.Items;

            //var discount = await _product.GetAllPaging(new GetProductPagingRequest()
            //{
            //    PageIndex = 1,
            //    PageSize = 8,
            //    IsDiscount = true
            //});
            //ViewBag.ProductDisCounts = discount.Items;

            var configCategory = new GetConfigPagingRequest()
            {
                PageIndex = 1,
                PageSize = 4,
                ConfigEnum = ConfigEnum.Category,
            };
            var configCategoryData = await _IConfigService.GetAllPaging(request: configCategory);

            var postType = await _postType.GetAllPaging(new GetPostTypePagingRequest()
            {
                PageIndex = 1,
                PageSize = 4,
                PostTypeEnum = PostTypeEnum.Warehouse,
                Ids = configCategoryData.Items.Select(m => m.ConfigId).ToList()
            });
            ViewBag.ProductTypes = postType.Items;
            var result = await _product.GetAllPaging(new GetProductPagingRequest()
            {
                PageIndex = 1,
                PageSize = 8,
            });
            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }


    }
}
