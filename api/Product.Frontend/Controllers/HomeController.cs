using Microsoft.AspNetCore.Mvc;
using Project.Enums;
using Project.Services.Posts.Interfaces;
using Project.Services.Products.Interfaces;
using Project.ViewModels.Posts.Manage;
using Project.ViewModels.Products.Manage;

namespace Product.Frontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _product;
        private readonly IPostService _post;
        public HomeController(ILogger<HomeController> logger, IProductService product, IPostService post)
        {
            _logger = logger;
            _product = product;
            _post = post;
        }

        public async Task<IActionResult> Index()
        {

            IConfigurationRoot configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            ViewBag.DomainApi = configurationBuilder["Domain:Api"];
            var request = new GetProductPagingRequest()
            {
                PageIndex = 1,
                PageSize = 24
            };
            var post = await _post.GetAllPaging(new GetPostPagingRequest
            {
                PageIndex = 1,
                PageSize = 3,
                WorkflowId = Workflow.COMPLETED
            });
            ViewBag.Posts = post.Items;
            var result = await _product.GetAllPaging(request);
            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }


    }
}
