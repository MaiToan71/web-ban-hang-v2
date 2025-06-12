using Microsoft.AspNetCore.Mvc;
using Project.Caching.Interfaces;
using Project.Services.TimeShifts;
using Project.Services.Users.Interfaces;
using Project.ViewModels;
using Project.ViewModels.TimeShifts;

namespace Project.API.Controllers
{
    public class TimeShiftsController : BaseController
    {
        private readonly IAppUserService _IUserService;
        private readonly ICachingExtension _ICachingExtension;
        private readonly ITimeShiftService _ITimeShiftservice;

        public TimeShiftsController(

            IAppUserService userService,
            ICachingExtension cachingExtension,
            ITimeShiftService ITimeShiftservice
            )
        : base(cachingExtension, userService)
        {
            _ITimeShiftservice = ITimeShiftservice;
            _IUserService = userService;
        }
        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> GetSearch([FromBody] GetTimeShiftPagingRequest request)
        {

            try
            {

                var data = await _ITimeShiftservice.GetAllPaging(request: request);
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

                var item = await _ITimeShiftservice.GetById(id);
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
        public async Task<ActionResult> AddNew([FromBody] TimeShiftCreateRequest request)
        {
            var postResult = new PostResult();
            try
            {

                bool item = await _ITimeShiftservice.AddNew(request, CurrentUserViewModel.UserId, CurrentUserViewModel.FullName);
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
        public async Task<ActionResult> Update([FromBody] TimeShiftUpdateRequest request)
        {
            var postResult = new PostResult();
            try
            {

                bool item = await _ITimeShiftservice.Update(request, CurrentUserViewModel.UserId, CurrentUserViewModel.FullName);
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
        [Route("updateWorkflow")]
        [HttpPut]
        public async Task<ActionResult> UpdateWorkflow([FromBody] TimeShiftUpdateRequest request)
        {
            var postResult = new PostResult();
            try
            {

                bool item = await _ITimeShiftservice.UpdateWorkflow(request, CurrentUserViewModel.UserId, CurrentUserViewModel.FullName);
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

                bool item = await _ITimeShiftservice.Delete(id, CurrentUserViewModel.UserId, CurrentUserViewModel.FullName);
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
