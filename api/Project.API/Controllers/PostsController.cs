using Microsoft.AspNetCore.Mvc;
using Project.Caching.Interfaces;
using Project.Services.Images;
using Project.Services.Posts.Interfaces;
using Project.Services.Users.Interfaces;
using Project.ViewModels;
using Project.ViewModels.Posts.Manage;

namespace Project.API.Controllers
{
    public class PostsController : BaseController
    {
        private readonly IAppUserService _IUserService;
        private readonly ICachingExtension _ICachingExtension;
        public readonly IPostService _IPostService;
        private readonly ILogger<PostsController> _logger;
        private readonly IImageService _IImageService;
        public PostsController(IPostService IPostService, ILogger<PostsController> logger, IImageService IImageService, IAppUserService userService,
            ICachingExtension cachingExtension) : base(cachingExtension, userService)
        {
            _IPostService = IPostService;
            _logger = logger;
            _IImageService = IImageService;
        }
        [Route("search")]
        [HttpPost]
        public async Task<ActionResult> Searching([FromBody] GetPostPagingRequest request)
        {
            try
            {
                request.UserId = CurrentUserViewModel.UserId;
                var data = await _IPostService.GetAllPaging(request);

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
            var postResult = new PostResult();
            try
            {
                var data = await _IPostService.GetById(id);
                if (data == null)
                {
                    return BadRequest("Cannot find product");
                }
                postResult.Data = data;
            }
            catch (Exception ex)
            {
                postResult.Status = false;
                postResult.Data = ex.Message;
            }
            return Ok(postResult);
        }

        [Route("add")]
        [HttpPost]
        public async Task<ActionResult> AddNew([FromForm] PostCreateRequest request)
        {
            var postResult = new PostResult();
            try
            {

                //  request.UserId = userId;
                var productId = await _IPostService.Create(request, CurrentUserViewModel.UserId);
                postResult.Data = productId;
            }
            catch (Exception ex)
            {
                postResult.Status = false;
                postResult.Data = ex.Message;
            }
            return Ok(postResult);
        }

        [Route("update")]
        [HttpPut]
        public async Task<ActionResult> Update([FromForm] PostUpdateRequest request)
        {
            var postResult = new PostResult();
            try
            {
                var productId = await _IPostService.Update(request, CurrentUserViewModel.UserId);

            }
            catch (Exception ex)
            {
                postResult.Status = false;
                postResult.Data = ex.Message;
            }
            return Ok(postResult);
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteById(int id)
        {
            var postResult = new PostResult();
            try
            {
                var data = await _IPostService.Delete(id);

            }
            catch (Exception ex)
            {
                postResult.Status = false;
                postResult.Data = ex.Message;
            }
            return Ok(postResult);
        }
        [Route("image")]
        [HttpDelete]
        public async Task<ActionResult> DeleteImage(int imageId, int postId)
        {
            var postResult = new PostResult();
            try
            {
                var data = await _IImageService.DeletePostImage(imageId, postId);

            }
            catch (Exception ex)
            {
                postResult.Status = false;
                postResult.Data = ex.Message;
            }
            return Ok(postResult);
        }

    }
}
