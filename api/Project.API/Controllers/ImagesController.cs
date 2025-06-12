using Microsoft.AspNetCore.Mvc;
using Project.Caching.Interfaces;
using Project.Services.Images;
using Project.Services.Users.Interfaces;
using Project.ViewModels;
using Project.ViewModels.Images;

namespace Project.API.Controllers
{

    public class ImagesController : BaseController
    {
        public readonly IImageService _imageServcie;
        private readonly IAppUserService _IUserService;
        private readonly ICachingExtension _ICachingExtension;
        public ImagesController(IImageService imageServcie, IAppUserService userService,
            ICachingExtension cachingExtension) : base(cachingExtension, userService)
        {
            _imageServcie = imageServcie;

        }

        [Route("statistics")]
        [HttpPost]
        public async Task<ActionResult> Statistics([FromBody] GetImagePagingRequest request)
        {
            var res = new PagedResult<dynamic>();
            try
            {
                var data = await _imageServcie.GetStatistics(request);
                res.Items = data;
            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Message = ex.Message;
            }

            return Ok(res);
        }


        [Route("search")]
        [HttpPost]
        public async Task<ActionResult> Searching([FromBody] GetImagePagingRequest request)
        {
            try
            {
                var data = await _imageServcie.GetAllPaging(request);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetImage(int id)
        {
            var postResult = new PostResult();
            try
            {
                var data = await _imageServcie.GetById(id);

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
        public async Task<IActionResult> AddNew([FromForm] ImageCreateRequest request)
        {
            var postResult = new PostResult();
            try
            {

                var isAdd = await _imageServcie.CreateImage(request, CurrentUserViewModel.UserId, CurrentUserViewModel.FullName);

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
        public async Task<IActionResult> Delete(int id)
        {
            var postResult = new PostResult();
            try
            {
                var isDelete = await _imageServcie.DeleteImage(id);

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
