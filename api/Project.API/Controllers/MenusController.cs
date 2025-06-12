using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Services.Menus;
using Project.ViewModels;
using Project.ViewModels.Menus;

namespace Project.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MenusController : ControllerBase
    {
        private readonly IMenusService _IMenusService;
        public MenusController(IMenusService IMenusService)
        {
            _IMenusService = IMenusService;
        }

        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> GetSearch([FromBody] GetMenuPagingRequest request)
        {

            try
            {
                var data = await _IMenusService.GetAllPaging(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll()
        {

            try
            {
                var userId = Guid.Parse(HttpContext.User.Identities.FirstOrDefault().Name);
                var data = await _IMenusService.GetAll(userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {

            try
            {
                var data = await _IMenusService.GetById(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("add")]
        [HttpPost]
        public async Task<ActionResult> AddNew([FromBody] MenuCreateRequest request)
        {
            var postResult = new PostResult();
            try
            {
                int item = await _IMenusService.AddNew(request);
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
        public async Task<ActionResult> Update([FromBody] MenuUpdateRequest request)
        {
            var postResult = new PostResult();
            try
            {
                int item = await _IMenusService.Update(request);
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
                int item = await _IMenusService.Delete(id);
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
