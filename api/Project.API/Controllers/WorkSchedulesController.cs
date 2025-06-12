using Microsoft.AspNetCore.Mvc;
using Project.Caching.Interfaces;
using Project.Data.Entities;
using Project.Services.Schedule;
using Project.Services.Users.Interfaces;
using Project.ViewModels;
using Project.ViewModels.Schedule;

namespace Project.API.Controllers
{
    public class WorkScheduleController : BaseController
    {
        private readonly IAppUserService _IUserService;
        private readonly ICachingExtension _ICachingExtension;
        private readonly IWorkScheduleService _IWorkScheduleService;

        public WorkScheduleController(
            IAppUserService userService,
            ICachingExtension cachingExtension,
            IWorkScheduleService iWorkScheduleService)
         : base(cachingExtension, userService)
        {
            _IWorkScheduleService = iWorkScheduleService;
            _IUserService = userService;
        }
        /// <summary>
        /// AdminUpdateCalendar
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("update-ad")]
        [HttpPost]
        public async Task<ActionResult> AdminUpdateCalendar([FromBody] UpdateWorkRecordAdminRequest request)
        {
            var postResult = new PostResult();
            try
            {
                var item = await _IWorkScheduleService.AdminUpdateCalendar(request, CurrentUserViewModel.UserId, CurrentUserViewModel.FullName);
                if (item == false)
                {
                    postResult.Status = false;
                }
                postResult.Data = item;
            }
            catch (Exception ex)
            {
                postResult.Status = false;
                postResult.Data = ex.Message;
            }
            return Ok(postResult);
        }
        /// <summary>
        /// EmployeeUpdateCalendar
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("update")]
        [HttpPost]
        public async Task<ActionResult> EmployeeUpdateCalendar([FromBody] UpdateWorkRecordRequest request)
        {
            var postResult = new PostResult();
            try
            {
                var item = await _IWorkScheduleService.EmployeeUpdateCalendar(request, CurrentUserViewModel.UserId, CurrentUserViewModel.FullName);
                if (item == false)
                {
                    postResult.Status = false;
                }
                postResult.Data = item;
            }
            catch (Exception ex)
            {
                postResult.Status = false;
                postResult.Data = ex.Message;
            }
            return Ok(postResult);
        }
        /// <summary>
        /// EmployeeCheckin
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("checkin")]
        [HttpPost]
        public async Task<ActionResult> EmployeeCheckin([FromBody] EmployeeCheckinRequest request)
        {
            var postResult = new PostResult();
            try
            {
                var item = await _IWorkScheduleService.EmployeeCheckin(request, CurrentUserViewModel.UserId, CurrentUserViewModel.FullName);
                if (item == false)
                {
                    postResult.Status = false;
                }
                postResult.Data = item;
            }
            catch (Exception ex)
            {
                postResult.Status = false;
                postResult.Data = ex.Message;
            }
            return Ok(postResult);
        }
        /// <summary>
        /// EmployeeCheckout
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("checkout")]
        [HttpPost]
        public async Task<ActionResult> EmployeeCheckout([FromBody] EmployeeCheckoutRequest request)
        {
            var postResult = new PostResult();
            try
            {
                var item = await _IWorkScheduleService.EmployeeCheckout(request, CurrentUserViewModel.UserId, CurrentUserViewModel.FullName);
                if (item == false)
                {
                    postResult.Status = false;
                }
                postResult.Data = item;
            }
            catch (Exception ex)
            {
                postResult.Status = false;
                postResult.Data = ex.Message;
            }
            return Ok(postResult);
        }

        /// <summary>
        /// GetSearch
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<Data.Entities.WorkRecord>))]
        public async Task<IActionResult> GetSearch([FromBody] GetWorkRecordPagingRequest request)
        {
            try
            {
                var data = await _IWorkScheduleService.GetAllPaging(request: request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// GetById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get_by_id/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WorkRecord))]
        public async Task<IActionResult> GetById(int id)
        {
            var postResult = new PostResult();
            try
            {
                var data = await _IWorkScheduleService.GetById(id);

                postResult.Data = data;
            }
            catch (Exception ex)
            {
                postResult.Status = false;
                postResult.Data = ex.Message;
            }
            return Ok(postResult);
        }

        /// <summary>
        /// GetById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WorkRecord))]
        public async Task<IActionResult> Delete(int id)
        {
            var postResult = new PostResult();
            try
            {
                var data = await _IWorkScheduleService.Delete(id);

                postResult.Data = data;
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
