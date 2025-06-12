using Microsoft.AspNetCore.Mvc;
using Project.Enums;
using Project.Services.Posts.Interfaces;
using Project.Services.PostTypes;
using Project.Services.Products.Interfaces;
using Project.ViewModel.PostTypes;
using Project.ViewModels.Posts.Manage;
using Project.ViewModels.Products.Manage;

namespace Product.Frontend.Controllers
{

    public class ProductController : Controller
    {
        private readonly IProductService _product;
        private readonly IPostTypeService _postType;
        private readonly IPostService _post;
        public ProductController(IProductService product, IPostTypeService postType,
            IPostService post)
        {
            _product = product;
            _postType = postType;
            _post = post;
        }
        [Route("san-pham/{slug}")]
        public async Task<IActionResult> Index(string slug, string? search = null, int page = 1, int size = 24, decimal? from = null, decimal? to = null, int? posttype = null)
        {
            if (page < 1)
            {
                page = 1;
            }
            size = 24;
            IConfigurationRoot configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            ViewBag.DomainApi = configurationBuilder["Domain:Api"];
            var request = new GetProductPagingRequest()
            {
                PageIndex = page,
                PageSize = size
            };

            if (slug == "tat-ca")
            {

            }
            request.Search = search;
            request.PostTypeId = posttype;
            request.SellingPriceFrom = from;
            request.SellingPriceTo = to;
            var result = await _product.GetAllPaging(request);
            // Tính TotalPages ở đây
            int totalPages = (int)Math.Ceiling((double)result.TotalRecord / size);

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = size;
            ViewBag.PrinceFrom = from;
            ViewBag.PrinceTo = to;
            ViewBag.PostType = posttype;
            ViewBag.Slug = slug;

            ViewBag.Search = search;

            var postTypes = await _postType.GetAllPaging(new GetPostTypePagingRequest
            {
                PageIndex = 1,
                PageSize = 1000,
                PostTypeEnum = PostTypeEnum.Warehouse,
            });
            ViewBag.PostTypes = postTypes.Items;

            var post = await _post.GetAllPaging(new GetPostPagingRequest
            {
                PageIndex = 1,
                PageSize = 3,
                WorkflowId = Workflow.COMPLETED
            });
            ViewBag.Posts = post.Items;


            return View(result);
        }

        [Route("san-pham/chi-tiet/{id}")]
        public async Task<IActionResult> Detail(int id)
        {

            var item = await _product.GetById(id);
            return View(item);
        }
    }
}
