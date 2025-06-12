using Microsoft.AspNetCore.Mvc;
using Project.Caching.Interfaces;
using Project.Services.Users.Interfaces;
using Project.Usermager.Services.Interfaces;
using Project.ViewModel.Usermanagers.Roles;
using Project.ViewModels;

namespace Project.API.Controllers
{


    public class RolesController : BaseController
    {
        private readonly IAppUserService _IUserService;
        private readonly ICachingExtension _ICachingExtension;
        private readonly IRoleService _IRoleService;
        public RolesController(IRoleService IRoleService, IAppUserService userService,
            ICachingExtension cachingExtension) : base(cachingExtension, userService)
        {
            _IRoleService = IRoleService;
        }

        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> GetSearch([FromBody] GetRolePagingRequest request)
        {

            try
            {
                var data = await _IRoleService.GetAllPaging(request);
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
            var postResult = new PostResult();
            try
            {
                var data = await _IRoleService.GetById(id);
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
        public async Task<ActionResult> AddNew([FromBody] RoleCreateRequest request)
        {
            var postResult = new PostResult();
            try
            {

                int item = await _IRoleService.AddNew(request);


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
        public async Task<ActionResult> Update([FromBody] RoleUpdateRequest request)
        {
            var postResult = new PostResult();
            try
            {

                int item = await _IRoleService.Update(request);


            }
            catch (Exception ex)
            {
                postResult.Status = false;
                postResult.Data = ex.Message;
            }
            return Ok(postResult);
        }

        [Route("delete")]
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var postResult = new PostResult();
            try
            {
                int item = await _IRoleService.Delete(id);

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
