using Microsoft.AspNetCore.Mvc;
using Project.Caching.Interfaces;
using Project.Services.TimeShiftTypes;
using Project.Services.Users.Interfaces;
using Project.ViewModels;
using Project.ViewModels.TimeShiftTypes;

namespace Project.API.Controllers
{

    public class TimeShiftTypesController : BaseController
    {
        private readonly IAppUserService _IUserService;
        private readonly ICachingExtension _ICachingExtension;
        private readonly ITimeShiftTypeService _ITimeShiftTypeservice;
        public TimeShiftTypesController(

           IAppUserService userService,
           ICachingExtension cachingExtension,
           ITimeShiftTypeService ITimeShiftTypeservice
           )
       : base(cachingExtension, userService)
        {
            _ITimeShiftTypeservice = ITimeShiftTypeservice;
            _IUserService = userService;
        }

        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> GetSearch([FromBody] GetTimeShiftTypePagingRequest request)
        {
            try
            {
                var data = await _ITimeShiftTypeservice.GetAllPaging(request: request);
                return Ok(data);
            }
            catch (Exception ex)
            {
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
                var item = await _ITimeShiftTypeservice.GetById(id);
                postResult.Data = item;
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
        public async Task<ActionResult> AddNew([FromBody] TimeShiftTypeCreateRequest request)
        {
            var postResult = new PostResult();
            try
            {
                bool item = await _ITimeShiftTypeservice.AddNew(request, CurrentUserViewModel.UserId, CurrentUserViewModel.FullName);
                if (!item)
                {
                    postResult.Status = false;
                    postResult.Data = "Cannot find item";
                }
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
        public async Task<ActionResult> Update([FromBody] TimeShiftTypeUpdateRequest request)
        {
            var postResult = new PostResult();
            try
            {
                bool item = await _ITimeShiftTypeservice.Update(request, CurrentUserViewModel.UserId, CurrentUserViewModel.FullName);
                if (!item)
                {
                    postResult.Status = false;
                    postResult.Data = "Cannot find item";
                }
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
                bool item = await _ITimeShiftTypeservice.Delete(id, CurrentUserViewModel.UserId, CurrentUserViewModel.FullName);
                if (!item)
                {
                    postResult.Status = false;
                    postResult.Data = "Cannot find item";
                }
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
