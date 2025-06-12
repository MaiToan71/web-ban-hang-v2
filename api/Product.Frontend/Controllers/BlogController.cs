using Microsoft.AspNetCore.Mvc;
using Project.Enums;
using Project.Services.Posts.Interfaces;
using Project.Services.PostTypes;
using Project.ViewModels.Posts.Manage;

namespace Product.Frontend.Controllers
{
    public class BlogController : Controller
    {
        private readonly IPostService _post;
        private readonly IPostTypeService _postType;
        public BlogController(IPostTypeService postType,
            IPostService post)
        {
            _postType = postType;
            _post = post;
        }
        [Route("tin-tuc")]
        public async Task<IActionResult> Index(string? search = null, int page = 1, int size = 24)
        {

            size = 24;
            IConfigurationRoot configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            ViewBag.DomainApi = configurationBuilder["Domain:Api"];
            var post = await _post.GetAllPaging(new GetPostPagingRequest
            {
                PageIndex = page,
                PageSize = (int)size,
                Keyword = search,
                WorkflowId = Workflow.COMPLETED
            });
            int totalPages = (int)Math.Ceiling((double)post.TotalRecord / size);

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = size;
            ViewBag.Search = search;
            return View(post);
        }

        [Route("chi-tiet/{slug}")]
        public async Task<IActionResult> Detail(string slug)
        {

            var item = await _post.GetByUrl(slug);
            return View(item);
        }
    }
}
