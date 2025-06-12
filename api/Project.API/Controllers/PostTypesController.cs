using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Services.PostTypes;
using Project.ViewModel.PostTypes;
using Project.ViewModels;

namespace Project.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PostTypesController : ControllerBase
    {
        private readonly IPostTypeService _postTypeService;
        private readonly ILogger<PostTypesController> _logger;
        public PostTypesController(IPostTypeService postTypeService,
            ILogger<PostTypesController> logger)
        {
            _postTypeService = postTypeService;
            _logger = logger;
        }

        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> GetSearch([FromBody] GetPostTypePagingRequest request)
        {

            try
            {
                var data = await _postTypeService.GetAllPaging(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Route("add")]
        [HttpPost]
        public async Task<ActionResult> AddNew([FromBody] PostTypeCreateRequest request)
        {
            var postResult = new PostResult();
            try
            {
                int category = await _postTypeService.Add(request);
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
        public async Task<ActionResult> Update([FromBody] PostTypeUpdateRequest request)
        {
            var postResult = new PostResult();
            try
            {
                int category = await _postTypeService.Update(request);

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
        public async Task<ActionResult> Delete(int id)
        {
            var postResult = new PostResult();
            try
            {

                int categoryId = await _postTypeService.Delete(id);

            }
            catch (Exception ex)
            {
                postResult.Status = false;
                postResult.Data = ex.Message;
            }
            return Ok(postResult);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult> GetById(int id)
        {
            var postResult = new PostResult();
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var category = await _postTypeService.FindById(id);

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
